using Units.Boids;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units.Combat
{
    public class ArcherEnemyCombat : EnemyCombat
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public EnemyArcherBoid archerBoid;

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

            if (Time.time > lastShotTime + shootCooldown && archerBoid.GetBoidState() == EnemyBoidState.Distracted)
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