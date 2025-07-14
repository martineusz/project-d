using Units.Boids.Allies;
using UnityEngine;

namespace Units.Combat.Allies
{
    public class AllyArcherCombat : AllyCombat
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public AllyArcherBoid archerBoid;

        private float shootCooldown = 2f;
        private float lastShotTime;

        protected override void OnCollisionStay2D(Collision2D collision)
        {
            // Disable melee damage from base class
        }

        private void Update()
        {
            if (Time.time > lastShotTime + shootCooldown && archerBoid.GetBoidState() == AllyBoidState.Aggressive)
            {
                Vector2 target = archerBoid.aggroTarget.transform.position;
                ShootAt(target);
                lastShotTime = Time.time;
            }
        }

        private void ShootAt(Vector2 target)
        {
            Vector2 shootDir = target - (Vector2)shootPoint.position;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<ArcherBullet>().Initialize(shootDir, true);
        }
    }
}