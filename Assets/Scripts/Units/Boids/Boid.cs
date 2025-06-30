using UnityEngine;
using System.Collections.Generic;

public abstract class Boid : MonoBehaviour
{
    protected BoidType Type;
    
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
    protected virtual void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Velocity = Random.insideUnitCircle.normalized * speed;
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
}

public enum BoidType
{
    Ally,
    Enemy,
    Neutral
}