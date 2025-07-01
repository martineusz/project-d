using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class AllyBoid : Boid
    {
        public Color colorIdle = Color.white;
        public Color colorFollowing = Color.blue;
        public Color colorAggresive = Color.blue;

        private BoidState _boidState = BoidState.Idle;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerSeparationRadius = 1.5f;
        public float playerAlignmentWeight = 1f;

        public float stopRadius = 2.0f;

        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.1f;
        private const float MaxSlowDown = 0.5f;

        private GameObject _aggroEnemy;

        private bool _wasFollowing = false;

        protected void Awake()
        {
            Type = BoidType.Ally;
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Update()
        {
            List<Boid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 followPlayer;
            Vector2 playerSeparation;
            Vector2 playerAlignment;
            if (_boidState == BoidState.Following)
            {
                followPlayer = ComputeFollowPlayer() * playerFollowWeight;
                playerSeparation = ComputePlayerSeparation() * playerSeparationWeight;
                playerAlignment = ComputePlayerAlignment() * playerAlignmentWeight;
            }
            else
            {
                followPlayer = Vector2.zero;
                playerSeparation = Vector2.zero;
                playerAlignment = Vector2.zero;
            }

            Vector2 acceleration =
                separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment;

            float slowDownFactor = ComputeSlowDownFactor();

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * (speed * slowDownFactor);

            transform.position += (Vector3)(Velocity * Time.deltaTime);
            transform.up = Velocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (_boidState == BoidState.Following)
                {
                    _wasFollowing = true;
                }

                _aggroEnemy = other.gameObject;
                SetBoidState(BoidState.Aggressive);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && other.gameObject == _aggroEnemy)
            {
                _aggroEnemy = null;
                
                if (_wasFollowing)
                {
                    _wasFollowing = false;
                    SetBoidState(BoidState.Following);
                }
                else
                {
                    SetBoidState(BoidState.Idle);
                }
            }
        }

        public BoidState GetBoidState()
        {
            return _boidState;
        }

        public void SetBoidState(BoidState newState)
        {
            _boidState = newState;

            switch (_boidState)
            {
                case BoidState.Idle:
                    SpriteRenderer.color = colorIdle;
                    break;
                case BoidState.Following:
                    SpriteRenderer.color = colorFollowing;
                    break;
                case BoidState.Aggressive:
                    SpriteRenderer.color = colorAggresive;
                    break;
            }
        }


        private float ComputeSlowDownFactor()
        {
            if (_boidState == BoidState.Idle) return minimumSlowDown;
            if (_boidState == BoidState.Aggressive) return 1f;
            if (!Player || !PlayerRb) return 1f;
            if (!(PlayerRb.linearVelocity.magnitude <= 0.001f)) return 1f;


            float distance = Vector2.Distance(transform.position, Player.position);

            if (distance < stopRadius)
            {
                return 0f;
            }

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

        protected List<Boid> GetNeighbors()
        {
            List<Boid> neighbors = new List<Boid>();
            foreach (Boid other in manager.allAllyBoids)
            {
                if (other == this) continue;
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < neighborRadius)
                    neighbors.Add(other);
            }

            return neighbors;
        }
    }

    public enum BoidState
    {
        Idle,
        Following,
        Aggressive
    }
}