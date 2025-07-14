using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Units.Boids.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyBoidPrefab;
        private Transform _player;
        public float spawnInterval = 5f;
        public float proximityRadius = 10f;
        public float spawnRadius = 5f;
        public int maxEnemies = 20;

        private float _timer;
        private List<GameObject> _spawnedEnemies = new List<GameObject>();

        private void Awake()
        {
            if (_player == null)
            {
                var playerController = FindFirstObjectByType<PlayerMovement>();
                if (playerController != null)
                    _player = playerController.transform;
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                if (_player && Vector3.Distance(transform.position, _player.position) <= proximityRadius)
                {
                    TrySpawnEnemy();
                    _timer = 0f;
                }
                
            }

            

            _spawnedEnemies.RemoveAll(e => !e);
        }

        private void TrySpawnEnemy()
        {
            if (_spawnedEnemies.Count >= maxEnemies) return;

            Vector2 spawnOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);

            GameObject enemy = Instantiate(enemyBoidPrefab, spawnPos, Quaternion.identity);
            _spawnedEnemies.Add(enemy);
        }
    }
}