using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class BoidManager : MonoBehaviour
    {
        public Transform player;

        public GameObject allyBoidPrefab;
        public GameObject enemyBoidPrefab;
        public GameObject workerBoidPrefab;

        public int allyBoidCount = 30;
        public int enemyBoidCount = 10;
        public int workerBoidCount = 5;
        public float spawnRadius = 10f;

        public Vector2 allySpawnCenter = new Vector2(0f, 0f);
        public Vector2 enemySpawnCenter = new Vector2(20f, 0f);
        public Vector2 workerSpawnCenter = new Vector2(0f, 0f);

        [HideInInspector] public List<AllyBoid> allAllyBoids = new List<AllyBoid>();
        [HideInInspector] public List<EnemyBoid> allEnemyBoids = new List<EnemyBoid>();
        [HideInInspector] public List<WorkerBoid> allWorkerBoids = new List<WorkerBoid>();

        private void Start()
        {
            SpawnAllyBoids();
            SpawnEnemyBoids();
            SpawnWorkerBoids();
        }

        private void SpawnEnemyBoids()
        {
            for (int i = 0; i < enemyBoidCount; i++)
            {
                Vector2 spawnPos = enemySpawnCenter + Random.insideUnitCircle * spawnRadius;
                GameObject boidGo = Instantiate(enemyBoidPrefab, spawnPos, Quaternion.identity);
                EnemyBoid boid = boidGo.GetComponent<EnemyBoid>();
                boid.manager = this;
            }
        }

        private void SpawnAllyBoids()
        {
            for (int i = 0; i < allyBoidCount; i++)
            {
                Vector2 spawnPos = allySpawnCenter + Random.insideUnitCircle * spawnRadius;
                GameObject boidGo = Instantiate(allyBoidPrefab, spawnPos, Quaternion.identity);
                AllyBoid boid = boidGo.GetComponent<AllyBoid>();
                boid.manager = this;
            }
        }

        private void SpawnWorkerBoids()
        {
            for (int i = 0; i < workerBoidCount; i++)
            {
                Vector2 spawnPos = workerSpawnCenter + Random.insideUnitCircle * spawnRadius;
                GameObject boidGo = Instantiate(workerBoidPrefab, spawnPos, Quaternion.identity);
                WorkerBoid boid = boidGo.GetComponent<WorkerBoid>();
                boid.manager = this;
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

        public void DeselectFirstSelectedWorkerBoid()
        {
            foreach (WorkerBoid boid in allWorkerBoids)
            {
                if (boid.GetBoidState() == WorkerBoidState.Following)
                {
                    boid.SetBoidState(WorkerBoidState.GoingToWork);
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

        public void SelectNearestUnselectedWorkerBoid()
        {
            WorkerBoid nearest = FindNearestUnselectedWorkerBoid();

            if (nearest)
            {
                nearest.SetBoidState(WorkerBoidState.Following);
            }
        }

        private WorkerBoid FindNearestUnselectedWorkerBoid()
        {
            WorkerBoid nearest = null;
            float minDistance = Mathf.Infinity;

            foreach (WorkerBoid boid in allWorkerBoids)
            {
                if (boid.GetBoidState() == WorkerBoidState.GoingToWork ||
                    boid.GetBoidState() == WorkerBoidState.Working)
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