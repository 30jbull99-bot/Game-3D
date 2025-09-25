using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EclipseProtocol.Enemies;

namespace EclipseProtocol.Managers
{
    public class SurvivalManager : MonoBehaviour
    {
        [System.Serializable]
        public class WaveDefinition
        {
            public string name;
            public List<GameObject> enemyPrefabs;
            public int enemyCount = 5;
            public float spawnInterval = 1.5f;
        }

        [SerializeField] private List<WaveDefinition> waveSequence = new();
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float interWaveDelay = 10f;

        private int currentWaveIndex;
        private bool survivalActive;
        private int aliveEnemies;

        public delegate void SurvivalEvent(int waveIndex);
        public event SurvivalEvent OnWaveStarted;
        public event SurvivalEvent OnWaveCleared;

        private void Start()
        {
            StartCoroutine(SurvivalLoop());
        }

        private IEnumerator SurvivalLoop()
        {
            survivalActive = true;
            currentWaveIndex = 0;

            while (survivalActive)
            {
                WaveDefinition wave = waveSequence[Mathf.Min(currentWaveIndex, waveSequence.Count - 1)];
                OnWaveStarted?.Invoke(currentWaveIndex + 1);
                yield return SpawnWave(wave);

                while (aliveEnemies > 0)
                {
                    yield return null;
                }

                OnWaveCleared?.Invoke(currentWaveIndex + 1);
                currentWaveIndex++;
                yield return new WaitForSeconds(interWaveDelay);
            }
        }

        private IEnumerator SpawnWave(WaveDefinition wave)
        {
            aliveEnemies = wave.enemyCount;
            for (int i = 0; i < wave.enemyCount; i++)
            {
                Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
                GameObject enemy = Instantiate(prefab, spawn.position, spawn.rotation);
                if (enemy.TryGetComponent(out EnemyLifetime lifetime))
                {
                    lifetime.OnDeath += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        private void HandleEnemyDeath()
        {
            aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        }
    }
}
