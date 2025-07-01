using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class EnemyBoid : Boid
    {
        public float playerFollowWeight = 6f;

        protected void Awake()
        {
            Type = BoidType.Enemy;
        }

        protected void Update()
        {
            List<Boid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 followPlayer = ComputeFollowPlayer() * playerFollowWeight;

            Vector2 acceleration = separation + alignment + cohesion + followPlayer;

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * speed;

            transform.position += (Vector3)(Velocity * Time.deltaTime);
            transform.up = Velocity;
        }
        
        protected List<Boid> GetNeighbors()
        {
            List<Boid> neighbors = new List<Boid>();
            foreach (Boid other in manager.allEnemyBoids)
            {
                if (other == this) continue;
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < neighborRadius)
                    neighbors.Add(other);
            }

            return neighbors;
        }
    }
}