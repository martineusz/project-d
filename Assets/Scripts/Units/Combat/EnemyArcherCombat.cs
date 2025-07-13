using UnityEngine;

namespace Units.Combat
{
    public class ArcherEnemyCombat : EnemyCombat
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;  // Set this in the prefab or scene

        private float shootCooldown = 2f;
        private float lastShotTime;

        protected override void OnCollisionStay2D(Collision2D collision)
        {
            // Disable melee damage from base class
        }

        private void Update()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < 10f && Time.time > lastShotTime + shootCooldown)
            {
                ShootAt(player.transform.position);
                lastShotTime = Time.time;
            }
        }

        private void ShootAt(Vector2 target)
        {
            Vector2 shootDir = target - (Vector2)shootPoint.position;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<ArcherBullet>().Initialize(shootDir);
        }
    }
}