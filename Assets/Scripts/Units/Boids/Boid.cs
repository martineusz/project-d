using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public abstract class Boid : MonoBehaviour
    {
        protected BoidType Type;

        protected Transform Player;
        protected Rigidbody2D PlayerRb;

        public float responsiveness = 5f;

        public float speed = 2f;
        public float neighborRadius = 3f;
        public float separationRadius = 1f;

        public float separationWeight = 1.5f;
        public float alignmentWeight = 1f;
        public float cohesionWeight = 1f;
        
        public float playerSeparationRadius = 1.5f;

        [HideInInspector] public BoidManager manager;
        protected SpriteRenderer SpriteRenderer;

        protected Vector2 Velocity;

        protected virtual void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Velocity = Random.insideUnitCircle.normalized * speed;
            AssignPlayer();
        }

        private void AssignPlayer()
        {
            if (manager != null && manager.player != null)
            {
                Player = manager.player;
                PlayerRb = Player.GetComponent<Rigidbody2D>();
                if (PlayerRb == null)
                {
                    Debug.LogWarning("Player Rigidbody2D not found, player alignment will be zero.");
                }
            }
            else
            {
                Debug.LogError("AllyBoid requires a player");
            }
        }

        protected virtual void Update()
        {
            List<Boid> neighbors = GetNeighbors();

            Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
            Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
            Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

            Vector2 acceleration = separation + alignment + cohesion;

            Velocity += acceleration * (Time.deltaTime * responsiveness);
            Velocity = Velocity.normalized * speed;

            transform.position += (Vector3)(Velocity * Time.deltaTime);
            transform.up = Velocity;
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

        protected Vector2 ComputeSeparation(List<Boid> neighbors)
        {
            Vector2 force = Vector2.zero;
            foreach (Boid other in neighbors)
            {
                float dist = Vector2.Distance(transform.position, other.transform.position);
                if (dist < separationRadius)
                    force += (Vector2)(transform.position - other.transform.position) / dist;
            }

            return force.normalized;
        }

        protected Vector2 ComputeAlignment(List<Boid> neighbors)
        {
            if (neighbors.Count == 0) return Vector2.zero;
            Vector2 avgVelocity = Vector2.zero;
            foreach (Boid other in neighbors)
                avgVelocity += other.Velocity;
            avgVelocity /= neighbors.Count;
            return (avgVelocity - Velocity).normalized;
        }

        protected Vector2 ComputeCohesion(List<Boid> neighbors)
        {
            if (neighbors.Count == 0) return Vector2.zero;
            Vector2 center = Vector2.zero;
            foreach (Boid other in neighbors)
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
            if (!Player) return Vector2.zero;
            return ((Vector2)Player.position - (Vector2)transform.position).normalized;
        }

        protected Vector2 ComputePlayerSeparation()
        {
            if (!Player) return Vector2.zero;

            float dist = Vector2.Distance(transform.position, Player.position);
            if (dist < playerSeparationRadius)
            {
                return ((Vector2)transform.position - (Vector2)Player.position) / dist;
            }

            return Vector2.zero;
        }

        protected Vector2 ComputePlayerAlignment()
        {
            if (!PlayerRb) return Vector2.zero;

            Vector2 playerVelocity = PlayerRb.linearVelocity;
            if (playerVelocity == Vector2.zero) return Vector2.zero;

            return playerVelocity.normalized;
        }
    }
    
    

    public enum BoidType
    {
        Ally,
        Enemy,
        Neutral
    }
}