using Units.Boids;
using UnityEngine;

namespace Units.Combat
{
    public class WorkerCombat : AbstractCombat
    {
        protected override void Die()
        {
            WorkerBoid boid = (WorkerBoid)Boid;
            IsAlive = false;
            boid.UnassignFromWorkspace();
            boid.manager.allWorkerBoids.Remove(boid);
            Destroy(gameObject);
        }
    }
}