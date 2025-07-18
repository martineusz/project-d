using UnityEngine;

namespace Environment.Buildable.Traps
{
    public class LandmineTrap : AbstractTrap
    {
        public TrapBombExplosion bombExplosion;
        public bool isArmed = true;
        public override void TrapEnter(Collider2D other)
        {
            if (other.isTrigger) return;
            if (!isArmed) return;
            
            bombExplosion.StartBomb();
            isArmed = false;
        }

        public override void TrapExit(Collider2D other)
        {
            //Nothing
        }

        public override void TrapStay(Collider2D other)
        {
            // Nothing
        }
    }
}