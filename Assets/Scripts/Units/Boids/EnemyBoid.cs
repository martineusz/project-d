using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class EnemyBoid : AbstractBoid
    {
        public float playerFollowWeight = 6f;
        private EnemyBoidState _enemyBoidState = EnemyBoidState.Chasing;

        public float allyFollowWeight = 6f;

        public Color colorChasing = Color.red;
        public Color colorDistracted = Color.red;


        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.1f;
        private const float MaxSlowDown = 0.5f;

        private bool _playerInRange = false;

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

        private Vector2 ComputeAcceleration()
        {
            List<AbstractBoid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 followPlayer = ComputeFollowPlayer() * playerFollowWeight;
            Vector2 followAlly = Vector2.zero;
            if (_enemyBoidState == EnemyBoidState.Distracted)
            {
                followPlayer = Vector2.zero;
                followAlly = ComputeFollowAlly() * allyFollowWeight;
            }

            Vector2 acceleration = separation + alignment + cohesion + followPlayer + followAlly;
            return acceleration;
        }

        private float ComputeSlowDownFactor()
        {
            if (_enemyBoidState == EnemyBoidState.Chasing) return 1f;
            float distance = Vector2.Distance(transform.position, AggroTarget.transform.position);

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
            return _enemyBoidState;
        }

        public void SetBoidState(EnemyBoidState newState)
        {
            _enemyBoidState = newState;

            switch (_enemyBoidState)
            {
                case EnemyBoidState.Chasing:
                    SpriteRenderer.color = colorChasing;
                    break;
                case EnemyBoidState.Distracted:
                    SpriteRenderer.color = colorDistracted;
                    break;
            }
        }

        private Vector2 ComputeFollowAlly()
        {
            if (!AggroTarget) return Vector2.zero;
            AIPath.destination = AggroTarget.transform.position;
            return AIPath.desiredVelocity.normalized;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_playerInRange) return;
            
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                SetBoidState(EnemyBoidState.Chasing);
                AggroTarget = null;
                return;
            }

            if (AggroTarget != null) return;

            if (other.CompareTag("Ally") && AggroTarget == null)
            {
                AggroTarget = other.gameObject;
                SetBoidState(EnemyBoidState.Distracted);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
            if (other.CompareTag("Ally") && other.gameObject == AggroTarget)
            {
                SetBoidState(EnemyBoidState.Chasing);
                AggroTarget = null;
            }
        }
    }

    public enum EnemyBoidState
    {
        Chasing,
        Distracted,
    }
}