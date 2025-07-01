using System;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        private float _hp;
        public float maxHp;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        
        public void TakeDamage(float damage)
        {
            _hp -= damage;
            if (_hp <= 0)
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