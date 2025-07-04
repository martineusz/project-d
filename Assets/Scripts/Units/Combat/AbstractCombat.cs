using Units.Boids;
using UnityEngine;


namespace Units.Combat
{
    public abstract class AbstractCombat : MonoBehaviour
    {
        protected AbstractBoid Boid;

        protected float Hp;
        public float maxHp;
        public float attackDamage;
        public float attackSpeed = 1f;
        protected bool IsAlive = true;

        protected float LastAttackTime = -Mathf.Infinity;

        protected virtual void Awake()
        {
            Boid = GetComponent<AbstractBoid>();
            Hp = maxHp;
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (Boid.GetAggroTarget() != null && collision.gameObject == Boid.GetAggroTarget())
            {
                DealDamageUnit(collision);
            }
        }

        private void TakeDamage(float damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                Die();
            }
        }

        protected abstract void Die();

        private void DealDamageUnit(Collision2D collision)
        {
            if ((Time.time - LastAttackTime < 1f / attackSpeed)) return;
            
            var enemyCombat = collision.gameObject.GetComponent<AbstractCombat>();
            if (enemyCombat == null) return;
            
            enemyCombat.TakeDamage(attackDamage);
            LastAttackTime = Time.time;
        }
    }
}