using System.Collections.Generic;
using UnityEngine;

public class AllyBoid : Boid
{
    private Transform _player;
    private Rigidbody2D _playerRb;
    
    public Color colorUnselected = Color.white;
    public Color colorSelected = Color.blue;
    
    private bool _selected;
    
    public float playerFollowWeight = 2f;
    public float playerSeparationWeight = 1.5f;
    public float playerAlignmentWeight = 1f;
    
    public float stopRadius = 2.0f;
    public float playerSeparationRadius = 1.5f;
    
    public float slowDownRadius = 1.5f;
    public float minimumSlowDown = 0.1f;
    private const float MaxSlowDown = 0.5f;
    
    protected void Awake()
    {
        Type = BoidType.Ally;
    }
    protected override void Start()
    {
        base.Start();

        if (manager != null && manager.player != null)
        {
            _player = manager.player;
            _playerRb = _player.GetComponent<Rigidbody2D>();
            if (_playerRb == null)
            {
                Debug.LogWarning("Player Rigidbody2D not found, player alignment will be zero.");
            }
        }
        else
        {
            Debug.LogError("AllyBoid requires a player");
        }
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


    private Vector2 ComputeFollowPlayer()
    {
        if (!_player) return Vector2.zero;
        return ((Vector2)_player.position - (Vector2)transform.position).normalized;
    }

    private Vector2 ComputePlayerSeparation()
    {
        if (!_player) return Vector2.zero;

        float dist = Vector2.Distance(transform.position, _player.position);
        if (dist < playerSeparationRadius)
        {
            return ((Vector2)transform.position - (Vector2)_player.position) / dist;
        }
        return Vector2.zero;
    }

    private Vector2 ComputePlayerAlignment()
    {
        if (!_playerRb) return Vector2.zero;

        Vector2 playerVelocity = _playerRb.linearVelocity;
        if (playerVelocity == Vector2.zero) return Vector2.zero;

        return playerVelocity.normalized;
    }

    private float ComputeSlowDownFactor()
    {
        if (!_player || !_playerRb) return 1f;

        float distance = Vector2.Distance(transform.position, _player.position);
        bool playerIsIdle = _playerRb.linearVelocity.magnitude <= 0.001f;
        
        
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
