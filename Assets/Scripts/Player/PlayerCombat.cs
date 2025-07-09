using System;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        public float hp;
        public float maxHp;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            hp = maxHp;
        }
        
        public void TakeDamage(float damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            //TODO: implement actual death logic
        }
    }
}