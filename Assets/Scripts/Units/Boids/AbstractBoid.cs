using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units.Boids
{
    public abstract class AbstractBoid : MonoBehaviour
    {
        protected BoidType Type;
        
        public GameObject aggroTarget;
        protected AIPath AIPath;

        protected Transform Player;
        protected Rigidbody2D PlayerRb;
        protected Rigidbody2D Rb;

        public float responsiveness = 5f;

        public float speed = 2f;
        public float neighborRadius = 3f;
        public float separationRadius = 1f;

        public float separationWeight = 1.5f;
        public float alignmentWeight = 1f;
        public float cohesionWeight = 1f;

        [HideInInspector] public BoidManager manager;
        protected SpriteRenderer SpriteRenderer;

        protected Vector2 Velocity;
        
        protected abstract void HandleMovement();
        protected abstract List<AbstractBoid> GetNeighbors();
        
        protected virtual void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody2D>();
            Velocity = Random.insideUnitCircle.normalized * speed;
            AIPath = GetComponent<AIPath>();
            
            AssignPlayer();
        }
        
        protected void FixedUpdate()
        {
            HandleMovement();
        }

        private void AssignPlayer()
        {
            if (manager != null && manager.player != null)
            {
                Player = manager.player;
                PlayerRb = Player.GetComponent<Rigidbody2D>();
            }
        }

        protected Vector2 ComputeSeparation(List<AbstractBoid> neighbors)
        {
            Vector2 force = Vector2.zero;
            foreach (AbstractBoid other in neighbors)
            {
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < separationRadius)
                    force += (Vector2)(transform.position - other.transform.position) / dist;
            }

            return force.normalized;
        }

        protected Vector2 ComputeAlignment(List<AbstractBoid> neighbors)
        {
            if (neighbors.Count == 0) return Vector2.zero;
            Vector2 avgVelocity = Vector2.zero;
            foreach (AbstractBoid other in neighbors)
                avgVelocity += other.Velocity;
            avgVelocity /= neighbors.Count;
            return (avgVelocity - Velocity).normalized;
        }

        protected Vector2 ComputeCohesion(List<AbstractBoid> neighbors)
        {
            if (neighbors.Count == 0) return Vector2.zero;
            Vector2 center = Vector2.zero;
            foreach (AbstractBoid other in neighbors)
                center += (Vector2)other.transform.position;
            center /= neighbors.Count;
            return (center - (Vector2)transform.position).normalized;
        }

        public BoidType GetBoidType()
        {
            return Type;
        }
        
        protected Vector2 ComputeFollowPlayer()
        {
            if (!Player || !AIPath) return Vector2.zero;
            AIPath.destination = Player.position;
            return AIPath.desiredVelocity.normalized;
        }
        
        public GameObject GetAggroTarget()
        {
            return aggroTarget;
        }
    }
    
    

    public enum BoidType
    {
        Ally,
        Enemy,
        Worker,
        Neutral
    }
}