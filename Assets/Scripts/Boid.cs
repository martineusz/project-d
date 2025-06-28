using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{
    public float speed = 2f;
    public float neighborRadius = 3f;
    public float separationRadius = 1f;

    public float separationWeight = 1.5f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;

    [HideInInspector] public BoidManager manager;

    private Vector2 velocity;

    void Start()
    {
        velocity = Random.insideUnitCircle.normalized * speed;
    }

    void Update()
    {
        List<Boid> neighbors = GetNeighbors();

        Vector2 separation = ComputeSeparation(neighbors) * separationWeight;
        Vector2 alignment = ComputeAlignment(neighbors) * alignmentWeight;
        Vector2 cohesion = ComputeCohesion(neighbors) * cohesionWeight;

        Vector2 acceleration = separation + alignment + cohesion;
        velocity += acceleration * Time.deltaTime;

        velocity = velocity.normalized * speed;
        transform.position += (Vector3)(velocity * Time.deltaTime);
        transform.up = velocity; // Rotate the sprite to face the direction
    }

    List<Boid> GetNeighbors()
    {
        List<Boid> neighbors = new List<Boid>();
        foreach (Boid other in manager.allBoids)
        {
            if (other == this) continue;
            float dist = Vector2.Distance(transform.position, other.transform.position);
            if (dist < neighborRadius)
                neighbors.Add(other);
        }
        return neighbors;
    }

    Vector2 ComputeSeparation(List<Boid> neighbors)
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

    Vector2 ComputeAlignment(List<Boid> neighbors)
    {
        if (neighbors.Count == 0) return Vector2.zero;
        Vector2 avgVelocity = Vector2.zero;
        foreach (Boid other in neighbors)
            avgVelocity += other.velocity;
        avgVelocity /= neighbors.Count;
        return (avgVelocity - velocity).normalized;
    }

    Vector2 ComputeCohesion(List<Boid> neighbors)
    {
        if (neighbors.Count == 0) return Vector2.zero;
        Vector2 center = Vector2.zero;
        foreach (Boid other in neighbors)
            center += (Vector2)other.transform.position;
        center /= neighbors.Count;
        return (center - (Vector2)transform.position).normalized;
    }
}
