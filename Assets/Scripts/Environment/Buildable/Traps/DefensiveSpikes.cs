using Units.Combat.Enemies;
using UnityEngine;
using System.Collections.Generic;

namespace Environment.Buildable
{
    public class DefensiveSpikes : AbstractTrap
    {
        public float damage = 10f;
        public float damageInterval = 1f;
        public float slowDownFactor = 0.9f;

        private Dictionary<EnemyCombat, float> damageTimers = new Dictionary<EnemyCombat, float>();

        public override void TrapEnter(Collider2D other)
        {
            if (other.isTrigger) return;

            var enemyBoid = other.GetComponent<Units.Boids.Enemies.EnemyBoid>();
            if (enemyBoid != null)
            {
                enemyBoid.externalSpeedFactor *= slowDownFactor;
            }

            var enemy = other.GetComponent<EnemyCombat>();
            if (enemy != null && !damageTimers.ContainsKey(enemy))
            {
                damageTimers[enemy] = -damageInterval;
            }
        }

        public override void TrapExit(Collider2D other)
        {
            if (other.isTrigger) return;

            var enemyBoid = other.GetComponent<Units.Boids.Enemies.EnemyBoid>();
            if (enemyBoid != null)
            {
                enemyBoid.externalSpeedFactor /= slowDownFactor;
            }

            var enemy = other.GetComponent<EnemyCombat>();
            if (enemy != null && damageTimers.ContainsKey(enemy))
            {
                damageTimers.Remove(enemy);
            }
        }

        public override void TrapStay(Collider2D other)
        {
            if (other.isTrigger) return;

            var enemy = other.GetComponent<EnemyCombat>();
            if (enemy == null) return;

            if (!damageTimers.ContainsKey(enemy))
            {
                damageTimers[enemy] = -damageInterval;
            }

            float lastDamageTime = damageTimers[enemy];
            if (Time.time - lastDamageTime >= damageInterval)
            {
                enemy.TakeDamage(damage);
                damageTimers[enemy] = Time.time;
            }
        }
    }
}
