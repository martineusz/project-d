using UnityEngine;

namespace Environment.Buildable
{
    public abstract class AbstractTrap : MonoBehaviour
    {
        public abstract void TrapEnter(Collider2D other);
        public abstract void TrapExit(Collider2D other);
        public abstract void TrapStay(Collider2D other);
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                TrapEnter(other);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                TrapExit(other);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                TrapStay(other);
            }
        }
    }
}