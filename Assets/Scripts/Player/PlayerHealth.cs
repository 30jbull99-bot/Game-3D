using UnityEngine;
using UnityEngine.Events;
using EclipseProtocol.Combat;

namespace EclipseProtocol.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 150f;
        [SerializeField] private UnityEvent<float, float> onHealthChanged;
        [SerializeField] private UnityEvent onPlayerDied;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            onHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0f)
            {
                onPlayerDied?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}
