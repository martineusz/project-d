using System.Collections.Generic;
using Units.Combat.Enemies;
using UnityEngine;

namespace Environment.Buildable.Towers
{
    public abstract class AbstractTower : MonoBehaviour
    {
        private List<EnemyCombat> enemiesInRange = new List<EnemyCombat>();
        protected EnemyCombat TargetEnemy;
        protected virtual void Update()
        {
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            float closestDistance = Mathf.Infinity;
            EnemyCombat closestEnemy = null;

            foreach (var enemy in enemiesInRange)
            {
                if (!enemy) continue;

                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            TargetEnemy = closestEnemy;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger) return;
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<EnemyCombat>();
                if (enemy != null && !enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger) return;
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<EnemyCombat>();
                if (enemy != null && enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Remove(enemy);
                }
            }
        }

        public EnemyCombat GetCurrentTarget()
        {
            return TargetEnemy;
        }
    }
}