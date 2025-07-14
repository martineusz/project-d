using Units.Boids;
using Units.Boids.Allies;

namespace Units.Combat.Allies
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