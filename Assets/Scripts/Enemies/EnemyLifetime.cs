using System;
using UnityEngine;
using EclipseProtocol.Combat;

namespace EclipseProtocol.Enemies
{
    public class EnemyLifetime : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 120f;

        public event Action OnDeath;
        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void ApplyDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
