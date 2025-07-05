using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class WorkerBoid : AbstractBoid
    {
        public Color colorIdle;
        public Color colorFollowing;
        public Color colorWorking;
        
        private Transform _targetWorkplace;

        private WorkerBoidState _workerBoidState = WorkerBoidState.GoingToWork;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerSeparationRadius = 1.5f;
        public float playerAlignmentWeight = 1f;
        
        public float workplaceFollowWeight = 6f;

        private Bounds _workplaceBounds = default;

        protected void Awake()
        {
            Type = BoidType.Worker;
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void HandleMovement()
        {
            if (_workerBoidState == WorkerBoidState.GoingToWork && !_targetWorkplace)
            {
                FindClosestWorkplace();
            }
            
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
            
            Vector2 workplaceFollow = Vector2.zero;
            Vector2 followPlayer = Vector2.zero;
            Vector2 playerSeparation = Vector2.zero;
            Vector2 playerAlignment = Vector2.zero;

            if (_workerBoidState == WorkerBoidState.GoingToWork)
            {
                workplaceFollow = ComputeWorkplaceFollow() * workplaceFollowWeight;
            }
            else if (_workerBoidState == WorkerBoidState.Following)
            {
                followPlayer = ComputeFollowPlayer() * playerFollowWeight;
                playerSeparation = ComputePlayerSeparation() * playerSeparationWeight;
                playerAlignment = ComputePlayerAlignment() * playerAlignmentWeight;
            }

            return separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment + workplaceFollow;
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

        private Vector2 ComputeWorkplaceFollow()
        {
            if (!_targetWorkplace || !AIPath) return Vector2.zero;
            AIPath.destination = _targetWorkplace.position;
            return AIPath.desiredVelocity.normalized;
        }
        
        private void FindClosestWorkplace()
        {
            GameObject[] workplaces = GameObject.FindGameObjectsWithTag("Workplace");
            if (workplaces.Length == 0) return;

            GameObject nearest = null;
            float minDist = float.MaxValue;
            Vector2 currentPosition = transform.position;

            foreach (var workplace in workplaces)
            {
                float dist = Vector2.Distance(currentPosition, workplace.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = workplace;
                }
            }

            if (nearest) _targetWorkplace = nearest.transform;
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
                WorkerBoidState.GoingToWork => colorIdle,
                WorkerBoidState.Following => colorFollowing,
                WorkerBoidState.Working => colorWorking,
                _ => SpriteRenderer.color
            };
        }
        
        
    }

    public enum WorkerBoidState
    {
        GoingToWork,
        Following,
        Working
    }
}