using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class WorkerBoid : AbstractBoid
    {
        public Color colorIdle;
        public Color colorFollowing;
        public Color colorWorking;

        private WorkerBoidState _workerBoidState = WorkerBoidState.Idle;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerSeparationRadius = 1.5f;
        public float playerAlignmentWeight = 1f;

        protected void Awake()
        {
            Type = BoidType.Worker;
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void HandleMovement()
        {
            Vector2 acceleration = ComputeAcceleration();

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * speed;

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

            if (_workerBoidState == WorkerBoidState.Following)
            {
                followPlayer = ComputeFollowPlayer() * playerFollowWeight;
                playerSeparation = ComputePlayerSeparation() * playerSeparationWeight;
                playerAlignment = ComputePlayerAlignment() * playerAlignmentWeight;
            }

            return separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment;
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

        protected override List<AbstractBoid> GetNeighbors()
        {
            List<AbstractBoid> neighbors = new List<AbstractBoid>();
            foreach (WorkerBoid other in manager.allWorkerBoids)
            {
                if (other == this) continue;
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < neighborRadius)
                    neighbors.Add(other);
            }

            return neighbors;
        }
        
        public WorkerBoidState GetBoidState()
        {
            return _workerBoidState;
        }
        
        public void SetBoidState(WorkerBoidState newState)
        {
            _workerBoidState = newState;

            SpriteRenderer.color = _workerBoidState switch
            {
                WorkerBoidState.Idle => colorIdle,
                WorkerBoidState.Following => colorFollowing,
                WorkerBoidState.Working => colorWorking,
                _ => SpriteRenderer.color
            };
        }
    }

    public enum WorkerBoidState
    {
        Idle,
        Following,
        Working
    }
}