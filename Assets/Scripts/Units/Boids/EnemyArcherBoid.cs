using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Units.Boids
{
    public class EnemyArcherBoid : EnemyBoid
    {
        protected override void HandleMovement()
        {
            if (EnemyBoidState == EnemyBoidState.Distracted)
            {
                AIPath.enabled = false;
                Velocity = Vector2.zero;
                return;
            }
            AIPath.enabled = true;
            
            Vector2 acceleration = ComputeAcceleration();
            float slowDownFactor = ComputeSlowDownFactor();

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * (speed * slowDownFactor);


            transform.position += (Vector3)(Velocity * Time.deltaTime);
            transform.up = Velocity;
        }

        protected override Vector2 ComputeAcceleration()
        {
            if (EnemyBoidState == EnemyBoidState.Distracted)
            {
                return Vector2.zero;
            }

            List<AbstractBoid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 followPlayer = ComputeFollowPlayer() * playerFollowWeight;

            Vector2 acceleration = separation + alignment + cohesion + followPlayer;

            return acceleration;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (PlayerInRange) return;


            if (other.CompareTag("Player"))
            {
                SetBoidState(EnemyBoidState.Distracted);
                PlayerInRange = true;
                AggroTarget = null;
                return;
            }

            if (other.CompareTag("Ally") && AggroTarget == null)
            {
                SetBoidState(EnemyBoidState.Distracted);
                AggroTarget = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SetBoidState(EnemyBoidState.Chasing);
                PlayerInRange = false;
            }

            if (other.CompareTag("Ally") && other.gameObject == AggroTarget)
            {
                SetBoidState(EnemyBoidState.Chasing);
                AggroTarget = null;
            }
        }
    }
}