using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EclipseProtocol.Combat
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponDefinition weapon;
        [SerializeField] private Transform muzzle;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] private Camera playerCamera;

        public int CurrentAmmo { get; private set; }
        public bool IsReloading { get; private set; }

        private float nextFireTime;
        private PlayerInput input;

        private void Awake()
        {
            input = GetComponentInParent<PlayerInput>();
        }

        private void OnEnable()
        {
            if (weapon != null)
            {
                CurrentAmmo = weapon.magazineSize;
            }

            if (input != null)
            {
                input.actions["Fire"].performed += OnFirePerformed;
                input.actions["Fire"].canceled += OnFireCanceled;
                input.actions["Reload"].performed += OnReloadPerformed;
            }
        }

        private void OnDisable()
        {
            if (input != null)
            {
                input.actions["Fire"].performed -= OnFirePerformed;
                input.actions["Fire"].canceled -= OnFireCanceled;
                input.actions["Reload"].performed -= OnReloadPerformed;
            }
        }

        private bool firingHeld;

        private void Update()
        {
            if (weapon == null || playerCamera == null)
            {
                return;
            }

            if (firingHeld && !IsReloading)
            {
                TryFire();
            }
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            firingHeld = true;
            TryFire();
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            firingHeld = false;
        }

        private void OnReloadPerformed(InputAction.CallbackContext context)
        {
            if (!IsReloading && CurrentAmmo < weapon.magazineSize)
            {
                StartCoroutine(ReloadRoutine());
            }
        }

        private void TryFire()
        {
            if (Time.time < nextFireTime || CurrentAmmo < weapon.ammoPerShot)
            {
                return;
            }

            nextFireTime = Time.time + 60f / weapon.fireRate;
            CurrentAmmo -= weapon.ammoPerShot;

            PlayFireEffects();

            if (weapon.isHitscan)
            {
                FireHitscan();
            }
            else
            {
                FireProjectile();
            }
        }

        private void PlayFireEffects()
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            if (audioSource != null && weapon.fireClip != null)
            {
                audioSource.PlayOneShot(weapon.fireClip);
            }

            if (weapon.muzzleVfxPrefab != null && muzzle != null)
            {
                Instantiate(weapon.muzzleVfxPrefab, muzzle.position, muzzle.rotation);
            }
        }

        private void FireHitscan()
        {
            Vector3 direction = GetBloomedDirection();
            if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, weapon.maxRange, hitMask))
            {
                if (weapon.impactVfxPrefab != null)
                {
                    Instantiate(weapon.impactVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(weapon.damage);
                }
            }
        }

        private void FireProjectile()
        {
            if (weapon.projectilePrefab == null || muzzle == null)
            {
                return;
            }

            GameObject projectile = Instantiate(weapon.projectilePrefab, muzzle.position, muzzle.rotation);
            if (projectile.TryGetComponent(out ProjectileBehaviour behaviour))
            {
                behaviour.Initialise(weapon.damage, GetBloomedDirection(), weapon.projectileSpeed, weapon.maxRange, hitMask);
            }
        }

        private Vector3 GetBloomedDirection()
        {
            Vector3 forward = playerCamera.transform.forward;
            float angle = weapon.bloomAngle;
            Vector3 random = Random.insideUnitSphere * Mathf.Tan(Mathf.Deg2Rad * angle);
            Vector3 direction = (forward + random).normalized;
            return direction;
        }

        private IEnumerator ReloadRoutine()
        {
            IsReloading = true;
            if (audioSource != null && weapon.reloadClip != null)
            {
                audioSource.PlayOneShot(weapon.reloadClip);
            }

            yield return new WaitForSeconds(weapon.reloadDuration);

            CurrentAmmo = weapon.magazineSize;
            IsReloading = false;
        }
    }
}
