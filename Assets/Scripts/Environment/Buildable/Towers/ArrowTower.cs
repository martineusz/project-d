using Units.Combat;
using Units.Combat.Enemies;
using UnityEngine;

namespace Environment.Buildable.Towers
{
    public class ArrowTower : AbstractTower
    {
        public GameObject arrowPrefab;
        public Transform shootPoint;
        
        public float shootCooldown = 2f;
        private float lastShotTime;
        
        protected override void Update()
        {
            base.Update();
            if (TargetEnemy && Time.time > lastShotTime + shootCooldown)
            {
                ShootAt(TargetEnemy.transform.position);
                lastShotTime = Time.time;
            }
        }
        
        private void ShootAt(Vector2 target)
        {
            Vector2 shootDir = target - (Vector2)shootPoint.position;
            GameObject bullet = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            bullet.GetComponent<ArcherBullet>().Initialize(shootDir, true);
        }
    }
}