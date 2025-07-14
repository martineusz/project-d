using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids.Enemies
{
    public class EnemyBoid : AbstractBoid
    {
        public float playerFollowWeight = 6f;
        protected EnemyBoidState EnemyBoidState = EnemyBoidState.Chasing;

        public float allyFollowWeight = 6f;

        public Color colorChasing = Color.red;
        public Color colorDistracted = Color.red;


        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.1f;
        protected const float MaxSlowDown = 0.5f;

        protected bool PlayerInRange = false;

        protected void Awake()
        {
            Type = BoidType.Worker;

            if (manager == null)
                manager = FindFirstObjectByType<BoidManager>();

            if (manager != null && !manager.allEnemyBoids.Contains(this))
                manager.allEnemyBoids.Add(this);
        }

        protected override void HandleMovement()
        {
            Vector2 acceleration = ComputeAcceleration();
            float slowDownFactor = ComputeSlowDownFactor();

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * (speed * slowDownFactor);


            transform.position += (Vector3)(Velocity * Time.deltaTime);
            transform.up = Velocity;
        }

        protected virtual Vector2 ComputeAcceleration()
        {
            List<AbstractBoid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 followPlayer = ComputeFollowPlayer() * playerFollowWeight;
            Vector2 followAlly = Vector2.zero;
            if (EnemyBoidState == EnemyBoidState.Distracted)
            {
                followPlayer = Vector2.zero;
                followAlly = ComputeFollowAlly() * allyFollowWeight;
            }

            Vector2 acceleration = separation + alignment + cohesion + followPlayer + followAlly;
            return acceleration;
        }

        protected float ComputeSlowDownFactor()
        {
            if (EnemyBoidState == EnemyBoidState.Chasing) return 1f;
            float distance = Vector2.Distance(transform.position, aggroTarget.transform.position);

            // if (distance < stopRadius)
            // {
            //     return 0f;
            // }

            if (distance < slowDownRadius)
            {
                float t = distance / slowDownRadius;
                return Mathf.Lerp(minimumSlowDown, MaxSlowDown, t);
            }

            return 1f;
        }

        protected override List<AbstractBoid> GetNeighbors()
        {
            List<AbstractBoid> neighbors = new List<AbstractBoid>();
            foreach (EnemyBoid other in manager.allEnemyBoids)
            {
                if (other == this) continue;
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < neighborRadius)
                    neighbors.Add(other);
            }

            return neighbors;
        }

        public EnemyBoidState GetBoidState()
        {
            return EnemyBoidState;
        }

        public void SetBoidState(EnemyBoidState newState)
        {
            EnemyBoidState = newState;

            switch (EnemyBoidState)
            {
                case EnemyBoidState.Chasing:
                    SpriteRenderer.color = colorChasing;
                    break;
                case EnemyBoidState.Distracted:
                    SpriteRenderer.color = colorDistracted;
                    break;
            }
        }

        protected Vector2 ComputeFollowAlly()
        {
            if (!aggroTarget) return Vector2.zero;
            AIPath.destination = aggroTarget.transform.position;
            return AIPath.desiredVelocity.normalized;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (PlayerInRange) return;
            
            if (other.CompareTag("Player"))
            {
                PlayerInRange = true;
                SetBoidState(EnemyBoidState.Chasing);
                aggroTarget = null;
                return;
            }

            if (other.CompareTag("Ally") && aggroTarget == null)
            {
                aggroTarget = other.gameObject;
                SetBoidState(EnemyBoidState.Distracted);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerInRange = false;
            }
            if (other.CompareTag("Ally") && other.gameObject == aggroTarget)
            {
                SetBoidState(EnemyBoidState.Chasing);
                aggroTarget = null;
            }
        }
    }

    public enum EnemyBoidState
    {
        Chasing,
        Distracted,
    }
}