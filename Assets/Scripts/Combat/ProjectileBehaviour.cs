using UnityEngine;

namespace EclipseProtocol.Combat
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        private float damage;
        private Vector3 direction;
        private float speed;
        private float maxDistance;
        private LayerMask hitMask;
        private Vector3 spawnPoint;

        [SerializeField] private GameObject impactVfxPrefab;

        public void Initialise(float damage, Vector3 direction, float speed, float maxDistance, LayerMask hitMask)
        {
            this.damage = damage;
            this.direction = direction.normalized;
            this.speed = speed;
            this.maxDistance = maxDistance;
            this.hitMask = hitMask;
            spawnPoint = transform.position;
        }

        private void Update()
        {
            float distance = speed * Time.deltaTime;
            Vector3 displacement = direction * distance;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, hitMask))
            {
                HandleHit(hit);
                return;
            }

            transform.position += displacement;

            if (Vector3.Distance(spawnPoint, transform.position) > maxDistance)
            {
                Destroy(gameObject);
            }
        }

        private void HandleHit(RaycastHit hit)
        {
            if (impactVfxPrefab != null)
            {
                Instantiate(impactVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
