using Units.Combat.Enemies;
using UnityEngine;

namespace Environment.Buildable.Traps
{
    public class TrapBombExplosion : MonoBehaviour
    {
        public float explosionDamage = 100f;
        public float explosionRadius = 10f;
        public LayerMask affectedLayers;
        public float explosionDelay = 2f;

        private bool _hasExploded = false;

        public void StartBomb()
        {
            Invoke(nameof(Explode), explosionDelay);
        }

        private void Explode()
        {
            _hasExploded = true;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);

            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    Debug.Log("Hit: " + col.name);
                    var enemy = col.GetComponent<EnemyCombat>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(explosionDamage);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}