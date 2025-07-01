using System.Collections.Generic;
using NUnit.Framework.Constraints;
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
        
        public bool isAlive = true;

        protected virtual void Awake()
        {
            Boid = GetComponent<AbstractBoid>();
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (Boid.GetAggroTarget() != null && collision.gameObject == Boid.GetAggroTarget())
            {
                DealDamage(collision);
            }
        }

        private void TakeDamage(float damage)
        {
            //TODO: damage taking animation, sound, etc.
            Hp -= damage;
            if (Hp <= 0)
            {
                Die();
            }
        }

        protected abstract void Die();


        private void DealDamage(Collision2D collision)
        {
            var enemyCombat = collision.gameObject.GetComponent<AbstractCombat>();
            if (enemyCombat != null)
            {
                enemyCombat.TakeDamage(attackDamage);
            }
        }
    }
}