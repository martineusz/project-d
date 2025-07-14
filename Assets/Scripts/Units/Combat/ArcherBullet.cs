using Player;
using UnityEngine;

namespace Units.Combat
{
    public class ArcherBullet : MonoBehaviour
    {
        public float speed = 5f;
        public int damage = 10;
        private Vector2 direction;

        public void Initialize(Vector2 shootDirection)
        {
            direction = shootDirection.normalized;
            Destroy(gameObject, 5f); // Auto-destroy after 5 sec
        }

        private void Update()
        {
            transform.Translate(direction * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            
            if (collision.CompareTag("Player"))
            {
                PlayerCombat pc = collision.GetComponent<PlayerCombat>();
                if (pc != null)
                {
                    pc.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Ally"))
            {
                AbstractCombat c = collision.GetComponent<AbstractCombat>();
                if (c != null)
                {
                    c.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                AbstractCombat c = collision.GetComponent<AbstractCombat>();
                if (c != null)
                {
                    c.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
    }
}