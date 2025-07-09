using System.Collections.Generic;
using Environment.Workplaces;
using UnityEngine;

namespace Units.Boids
{
    public class WorkerBoid : AbstractBoid
    {
        public Color colorIdle;
        public Color colorFollowing;
        public Color colorWorking;

        private Transform _targetWorkplace;

        public Transform TargetWorkplace => _targetWorkplace;

        private WorkerBoidState _workerBoidState = WorkerBoidState.GoingToWork;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerSeparationRadius = 1.5f;
        public float playerAlignmentWeight = 1f;

        public float workplaceFollowWeight = 6f;
        
        public float boundaryBuffer = 0.5f;
        public float boundarySeparationWeight = 5f;
        
        
        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.2f;
        public float workingSlowDown = 0.25f;
        private const float MaxSlowDown = 0.5f;
        

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

            Vector2 workplaceFollow = Vector2.zero;
            Vector2 followPlayer = Vector2.zero;
            Vector2 playerSeparation = Vector2.zero;
            Vector2 playerAlignment = Vector2.zero;
            Vector2 boundarySeparation = Vector2.zero;

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
            else if (_workerBoidState == WorkerBoidState.Working)
            {
                boundarySeparation = ComputeBoundarySeparation() * boundarySeparationWeight;
            }

            return separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment +
                   workplaceFollow + boundarySeparation;
        }
        
        private float ComputeSlowDownFactor()
        {
            if (_workerBoidState == WorkerBoidState.Working) return workingSlowDown;
            if (_workerBoidState == WorkerBoidState.GoingToWork) return 1f;
            if (_workerBoidState == WorkerBoidState.Following && (PlayerRb.linearVelocity.magnitude > 0.001f)) return 1f;
            
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
        private Vector2 ComputeBoundarySeparation()
        {
            if (_workplaceBounds == default) return Vector2.zero;

            Vector2 force = Vector2.zero;
            Vector2 pos = transform.position;

            if (pos.x < _workplaceBounds.min.x + boundaryBuffer)
                force.x += 1f;
            else if (pos.x > _workplaceBounds.max.x - boundaryBuffer)
                force.x -= 1f;

            if (pos.y < _workplaceBounds.min.y + boundaryBuffer)
                force.y += 1f;
            else if (pos.y > _workplaceBounds.max.y - boundaryBuffer)
                force.y -= 1f;

            return force.normalized;
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
                    IWorkplace iworkplace = workplace.gameObject.GetComponent<IWorkplace>();
                    if (iworkplace.QueueWorker(this))
                    {
                        minDist = dist;
                        nearest = workplace;
                    }
                }
            }

            if (nearest)
            {
                _targetWorkplace = nearest.transform;
                Collider2D targetCollider = nearest.GetComponent<Collider2D>();
                if (targetCollider)
                {
                    _workplaceBounds = targetCollider.bounds;
                }
            }
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
            if (newState == WorkerBoidState.Following && _workerBoidState == WorkerBoidState.GoingToWork)
            {
                UnassignFromQueue();
            }
            if (newState == WorkerBoidState.Following && _workerBoidState == WorkerBoidState.Working)
            {
                UnassignFromWorkspace();
            }
            if (newState != WorkerBoidState.Working) _targetWorkplace = null;
            _workerBoidState = newState;

            SpriteRenderer.color = _workerBoidState switch
            {
                WorkerBoidState.GoingToWork => colorIdle,
                WorkerBoidState.Following => colorFollowing,
                WorkerBoidState.Working => colorWorking,
                _ => SpriteRenderer.color
            };
        }

        public void UnassignFromWorkspace()
        {
            if (!_targetWorkplace) return; 

            var workplace = _targetWorkplace.GetComponent<Environment.Workplaces.IWorkplace>();
            workplace?.RemoveWorker(this);
        }
        
        public void UnassignFromQueue()
        {
            Debug.Log("Inassigning from queue");
            if (!_targetWorkplace) return; 

            
            var workplace = _targetWorkplace.GetComponent<Environment.Workplaces.IWorkplace>();
            workplace?.UnqueueWorker(this);
        }
    }

    public enum WorkerBoidState
    {
        GoingToWork,
        Following,
        Working
    }
}