using Units.Boids;
using UnityEngine;

namespace Units.Combat
{
    public class EnemyCombat : AbstractCombat
    {
        protected override void Die()
        {
            isAlive = false;
            Boid.manager.allEnemyBoids.Remove((EnemyBoid)Boid);
            Destroy(gameObject);
        }
    }
}