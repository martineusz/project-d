using System.Collections.Generic;
using UnityEngine;

namespace Units.Boids
{
    public class AllyBoid : Boid
    {
        public Color colorUnselected = Color.white;
        public Color colorSelected = Color.blue;

        private bool _selected = false;

        public float playerFollowWeight = 2f;
        public float playerSeparationWeight = 1.5f;
        public float playerAlignmentWeight = 1f;

        public float stopRadius = 2.0f;

        public float slowDownRadius = 1.5f;
        public float minimumSlowDown = 0.1f;
        private const float MaxSlowDown = 0.5f;

        protected void Awake()
        {
            Type = BoidType.Ally;
        }

        protected override void Update()
        {
            if (_selected)
            {
                List<Boid> neighbors = GetNeighbors();

                Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
                Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
                Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

                Vector2 followPlayer = ComputeFollowPlayer() * playerFollowWeight;
                Vector2 playerSeparation = ComputePlayerSeparation() * playerSeparationWeight;
                Vector2 playerAlignment = ComputePlayerAlignment() * playerAlignmentWeight;

                Vector2 acceleration =
                    separation + alignment + cohesion + followPlayer + playerSeparation + playerAlignment;

                float slowDownFactor = ComputeSlowDownFactor();

                Velocity += acceleration * (Time.deltaTime * responsiveness);
                Velocity = Velocity.normalized * (speed * slowDownFactor);

                transform.position += (Vector3)(Velocity * Time.deltaTime);
                transform.up = Velocity;
            }
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                SpriteRenderer.color = _selected ? colorSelected : colorUnselected;
            }
        }


        private float ComputeSlowDownFactor()
        {
            if (!Player || !PlayerRb) return 1f;

            float distance = Vector2.Distance(transform.position, Player.position);
            bool playerIsIdle = PlayerRb.linearVelocity.magnitude <= 0.001f;


            if (!playerIsIdle)
            {
                return 1f;
            }

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
    }
}