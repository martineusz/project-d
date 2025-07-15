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

            // Rotate the bullet to face the direction.
            // Assuming the bullet sprite faces UP (Vector3.up)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            Destroy(gameObject, 5f); // auto destroy after 5 sec
        }

        private void Update()
        {
            // Move forward along the local UP axis (bullet’s forward)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
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
            else if (collision.CompareTag("Ally") && !_isAllied)
            {
                AbstractCombat c = collision.GetComponent<AbstractCombat>();
                if (c != null)
                {
                    c.TakeDamage(damage);
                }

                Destroy(gameObject);
            }
            else if (collision.CompareTag("Enemy") && _isAllied)
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