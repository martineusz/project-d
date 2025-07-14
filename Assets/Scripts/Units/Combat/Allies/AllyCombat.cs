using Units.Boids;
using Units.Boids.Allies;

namespace Units.Combat.Allies
{
    public class AllyCombat : AbstractCombat
    {
        protected override void Die()
        {
            IsAlive = false;
            Boid.manager.allAllyBoids.Remove((AllyBoid)Boid);
            Destroy(gameObject);
        }
    }
}