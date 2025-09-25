using UnityEngine;

namespace EclipseProtocol.Combat
{
    [CreateAssetMenu(menuName = "Eclipse Protocol/Weapons/Weapon Definition")]
    public class WeaponDefinition : ScriptableObject
    {
        public string weaponName;
        public Sprite icon;
        [TextArea] public string description;

        [Header("Ballistics")]
        public bool isHitscan = true;
        public float damage = 35f;
        public float fireRate = 600f; // rounds per minute
        public float projectileSpeed = 140f;
        public float maxRange = 100f;
        public float bloomAngle = 0.25f;
        public AnimationCurve recoilPattern = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Ammo")]
        public int magazineSize = 30;
        public int ammoPerShot = 1;
        public float reloadDuration = 1.8f;

        [Header("Audio/Visual")]
        public AudioClip fireClip;
        public AudioClip reloadClip;
        public GameObject muzzleVfxPrefab;
        public GameObject impactVfxPrefab;
        public GameObject projectilePrefab;

        [Header("Gameplay")]
        public bool automatic = true;
        public bool allowAds = true;
        public float adsFov = 70f;
        public float hipFov = 90f;
    }
}
