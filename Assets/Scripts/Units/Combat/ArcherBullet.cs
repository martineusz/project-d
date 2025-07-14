using Player;
using UnityEngine;

namespace Units.Combat
{
    public class ArcherBullet : MonoBehaviour
    {
        public float speed = 5f;
        public int damage = 10;
        private Vector2 direction;
        private bool _isAllied = false;

        public void Initialize(Vector2 shootDirection, bool isAllied)
        {
            _isAllied = isAllied;
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
            
            if (collision.CompareTag("Player") && !_isAllied)
            {
                PlayerCombat pc = collision.GetComponent<PlayerCombat>();
                if (pc != null)
                {
                    pc.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Ally")&& !_isAllied)
            {
                AbstractCombat c = collision.GetComponent<AbstractCombat>();
                if (c != null)
                {
                    c.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Enemy")&& _isAllied)
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