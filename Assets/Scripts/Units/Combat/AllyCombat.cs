using Units.Boids;
using UnityEngine;

namespace Units.Combat
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