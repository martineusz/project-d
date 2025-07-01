using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class BoidManager : MonoBehaviour
    {   
        public Transform player;
        public GameObject allyBoidPrefab;
        public GameObject enemyBoidPrefab;
        public int allyBoidCount = 30;
        public int enemyBoidCount = 10;
        public float spawnRadius = 10f;
        public Vector2 enemySpawnCenter = new Vector2(20f, 0f); // Set this to your desired enemy spawn area

        [HideInInspector] public List<AllyBoid> allAllyBoids = new List<AllyBoid>();
        [HideInInspector] public List<EnemyBoid> allEnemyBoids = new List<EnemyBoid>();

        private void Start()
        {
            SpawnAllyBoids();
            SpawnEnemyBoids();
        }
        
        private void SpawnEnemyBoids()
        {
            for (int i = 0; i < enemyBoidCount; i++)
            {
                Vector2 spawnPos = enemySpawnCenter + Random.insideUnitCircle * spawnRadius;
                GameObject boidGo = Instantiate(enemyBoidPrefab, spawnPos, Quaternion.identity);
                EnemyBoid boid = boidGo.GetComponent<EnemyBoid>();
                boid.manager = this;
                allEnemyBoids.Add(boid);
            }
        }
        
        private void SpawnAllyBoids()
        {
            for (int i = 0; i < allyBoidCount; i++)
            {
                Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
                GameObject boidGo = Instantiate(allyBoidPrefab, spawnPos, Quaternion.identity);
                AllyBoid boid = boidGo.GetComponent<AllyBoid>();
                boid.manager = this;
                allAllyBoids.Add(boid);
            }
        }
    
        public void DeselectFirstSelectedAllyBoid()
        {
            foreach (AllyBoid boid in allAllyBoids)
            {
                if (boid.GetBoidState() == AllyBoidState.Following)
                {
                    boid.SetBoidState(AllyBoidState.Idle);
                    return;
                }
            }
        }
        
        public void SelectNearestUnselectedAllyBoid()
        {
            AllyBoid nearest = FindNearestUnselectedAllyBoid();

            if (nearest)
            {
                nearest.SetBoidState(AllyBoidState.Following);
            }
        }

        private AllyBoid FindNearestUnselectedAllyBoid()
        {
            AllyBoid nearest = null;
            float minDistance = Mathf.Infinity;

            foreach (AllyBoid boid in allAllyBoids)
            {
                if (boid.GetBoidState() == AllyBoidState.Idle)
                {
                    float dist = Vector3.Distance(player.position, boid.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        nearest = boid;
                    }
                }
            }

            return nearest;
        }
    }
}