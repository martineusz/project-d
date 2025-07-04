using Units.Boids;
using UnityEngine;

namespace Units.Combat
{
    public class WorkerCombat : AbstractCombat
    {
        protected override void Die()
        {
            IsAlive = false;
            Boid.manager.allWorkerBoids.Remove((WorkerBoid)Boid);
            Destroy(gameObject);
        }
    }
}