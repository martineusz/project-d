using UnityEngine;
using System.Collections.Generic;

public class BoidManager : MonoBehaviour
{   
    public Transform player;
    public GameObject boidPrefab;
    public int boidCount = 50;
    public float spawnRadius = 10f;

    [HideInInspector] public List<AllyBoid> allAllyBoids = new List<AllyBoid>();

    private void Start()
    {
        SpawnAllyBoids();
    }

    private void SpawnAllyBoids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
            GameObject boidGo = Instantiate(boidPrefab, spawnPos, Quaternion.identity);
            AllyBoid boid = boidGo.GetComponent<AllyBoid>();
            boid.manager = this;
            allAllyBoids.Add(boid);
        }
    }
        
    
    public void SelectNearestUnselectedAllyBoid()
    {
        AllyBoid nearest = FindNearestSelectedAllyBoid();

        if (nearest != null)
        {
            nearest.selected = true;
            Debug.Log("Selected nearest boid: " + nearest.name);
        }
        else
        {
            Debug.Log("No unselected boids found.");
        }
    }

    private AllyBoid FindNearestSelectedAllyBoid()
    {
        AllyBoid nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (AllyBoid boid in allAllyBoids)
        {
            if (!boid.selected)
            {
                float dist = Vector3.Distance(player.position, boid.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = boid;
                }
            }
        }

        return nearest;
    }
}