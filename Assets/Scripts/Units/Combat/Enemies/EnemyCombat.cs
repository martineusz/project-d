using Player;
using Units.Boids;
using Units.Boids.Enemies;
using UnityEngine;

namespace Units.Combat.Enemies
{
    public class EnemyCombat : AbstractCombat
    {
        public float fearOfLightModifier = 1f;
        
        protected override void Die()
        {
            IsAlive = false;
            Boid.manager.allEnemyBoids.Remove((EnemyBoid)Boid);
            Destroy(gameObject);
        }

        protected override void OnCollisionStay2D(Collision2D collision)
        {
            base.OnCollisionStay2D(collision);
            if (collision.gameObject.CompareTag("Player"))
            {
                DealDamagePlayer(collision.gameObject.GetComponent<PlayerCombat>());
            }
        }

        private void DealDamagePlayer(PlayerCombat pc)
        {
            if (!(Time.time - LastAttackTime >= 1f / attackSpeed)) return;
            
            pc.TakeDamage(attackDamage);
            LastAttackTime = Time.time;
        }
    }
}