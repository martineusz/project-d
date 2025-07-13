using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class AllyBoid : AbstractBoid
    {
        public Color colorIdle = Color.white;
        public Color colorFollowing = Color.blue;
        public Color colorAggresive = Color.blue;

        private AllyBoidState _allyBoidState = AllyBoidState.Idle;

        public float enemyFollowWeight = 6f;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerSeparationRadius = 1.5f;
        public float playerAlignmentWeight = 1f;

        //public float stopRadius = 2.0f;

        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.1f;
        private const float MaxSlowDown = 0.5f;

        private bool _wasFollowing = false;

        protected void Awake()
        {
            Type = BoidType.Ally;
            SpriteRenderer = GetComponent<SpriteRenderer>();

            if (manager == null)
                manager = FindFirstObjectByType<BoidManager>();

            if (manager != null && !manager.allAllyBoids.Contains(this))
                manager.allAllyBoids.Add(this);
        }

        protected override void HandleMovement()
        {
            Vector2 acceleration = ComputeAcceleration();
            float slowDownFactor = ComputeSlowDownFactor();

            float resp = responsiveness;
            if (_allyBoidState == AllyBoidState.Aggressive)
            {
                resp *= 2f;
            }

            Velocity += acceleration * (Time.deltaTime * resp);
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

            Vector2 followPlayer = Vector2.zero;
            Vector2 playerSeparation = Vector2.zero;
            Vector2 playerAlignment = Vector2.zero;
            Vector2 followEnemy = Vector2.zero;
            if (_allyBoidState == AllyBoidState.Following)
            {
                followPlayer = ComputeFollowPlayer() * playerFollowWeight;
                playerSeparation = ComputePlayerSeparation() * playerSeparationWeight;
                playerAlignment = ComputePlayerAlignment() * playerAlignmentWeight;
            }
            else if (_allyBoidState == AllyBoidState.Aggressive)
            {
                followEnemy = ComputeFollowEnemy() * enemyFollowWeight;
            }

            Vector2 acceleration =
                separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment + followEnemy;
            return acceleration;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy") || AggroTarget != null) return;
            if (_allyBoidState == AllyBoidState.Following)
            {
                _wasFollowing = true;
            }

            AggroTarget = other.gameObject;
            SetBoidState(AllyBoidState.Aggressive);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && other.gameObject == AggroTarget)
            {
                AggroTarget = null;

                if (_wasFollowing)
                {
                    _wasFollowing = false;
                    SetBoidState(AllyBoidState.Following);
                }
                else
                {
                    SetBoidState(AllyBoidState.Idle);
                }
            }
        }

        public AllyBoidState GetBoidState()
        {
            return _allyBoidState;
        }

        public void SetBoidState(AllyBoidState newState)
        {
            _allyBoidState = newState;

            SpriteRenderer.color = _allyBoidState switch
            {
                AllyBoidState.Idle => colorIdle,
                AllyBoidState.Following => colorFollowing,
                AllyBoidState.Aggressive => colorAggresive,
                _ => SpriteRenderer.color
            };
        }


        private float ComputeSlowDownFactor()
        {
            if (_allyBoidState == AllyBoidState.Idle) return minimumSlowDown;
            if (_allyBoidState == AllyBoidState.Following && (PlayerRb.linearVelocity.magnitude > 0.001f)) return 1f;
            if (_allyBoidState == AllyBoidState.Aggressive) return 1f;
            float distance = Vector2.Distance(transform.position, Player.position);

            if (distance < slowDownRadius)
            {
                float t = distance / slowDownRadius;
                return Mathf.Lerp(minimumSlowDown, MaxSlowDown, t);
            }

            return 1f;
        }

        private Vector2 ComputePlayerSeparation()
        {
            if (!Player) return Vector2.zero;

            float dist = Vector2.Distance(transform.position, Player.position);
            if (dist < playerSeparationRadius)
            {
                return ((Vector2)transform.position - (Vector2)Player.position) / dist;
            }

            return Vector2.zero;
        }

        private Vector2 ComputePlayerAlignment()
        {
            if (!PlayerRb) return Vector2.zero;

            Vector2 playerVelocity = PlayerRb.linearVelocity;
            if (playerVelocity == Vector2.zero) return Vector2.zero;

            return playerVelocity.normalized;
        }

        private Vector2 ComputeFollowEnemy()
        {
            if (!AggroTarget) return Vector2.zero;
            AIPath.destination = AggroTarget.transform.position;
            return AIPath.desiredVelocity.normalized;
        }

        protected override List<AbstractBoid> GetNeighbors()
        {
            List<AbstractBoid> neighbors = new List<AbstractBoid>();
            foreach (AllyBoid other in manager.allAllyBoids)
            {
                if (other == this) continue;
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < neighborRadius)
                    neighbors.Add(other);
            }

            return neighbors;
        }
        
        
    }

    public enum AllyBoidState
    {
        Idle,
        Following,
        Aggressive
    }
}