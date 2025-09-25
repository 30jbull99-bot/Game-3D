using UnityEngine;
using UnityEngine.AI;
using EclipseProtocol.Combat;

namespace EclipseProtocol.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float attackDamage = 15f;
        [SerializeField] private float attackRate = 1.2f;
        [SerializeField] private float detectionRadius = 25f;
        [SerializeField] private float stoppingDistance = 2.5f;

        [Header("FX")]
        [SerializeField] private GameObject deathVfxPrefab;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip deathClip;

        private NavMeshAgent agent;
        private Transform target;
        private float currentHealth;
        private float attackTimer;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            FindTarget();
        }

        private void Update()
        {
            if (target == null)
            {
                FindTarget();
                return;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= detectionRadius)
            {
                agent.SetDestination(target.position);
            }

            if (distance <= stoppingDistance)
            {
                agent.isStopped = true;
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackRate)
                {
                    attackTimer = 0f;
                    if (target.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.ApplyDamage(attackDamage);
                    }
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }

        private void FindTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }

        public void ApplyDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (deathVfxPrefab != null)
            {
                Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
            }

            if (audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }

            Destroy(gameObject);
        }
    }
}
