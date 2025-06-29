using UnityEngine;
using System.Collections.Generic;

public class BoidManager : MonoBehaviour
{   
    public Transform player;
    public GameObject boidPrefab;
    public int boidCount = 50;
    public float spawnRadius = 10f;

    [HideInInspector] public List<Boid> allBoids = new List<Boid>();

    private void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
            GameObject boidGO = Instantiate(boidPrefab, spawnPos, Quaternion.identity);
            Boid boid = boidGO.GetComponent<Boid>();
            boid.manager = this;
            allBoids.Add(boid);
        }
    }
}