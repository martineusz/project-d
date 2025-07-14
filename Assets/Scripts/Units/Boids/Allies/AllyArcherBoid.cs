using UnityEngine;

namespace Units.Boids.Allies
{
    public class AllyArcherBoid : AllyBoid
    {
        protected override void HandleMovement()
        {
            AIPath.enabled = true;
            if (AllyBoidState == AllyBoidState.Aggressive)
            {
                AIPath.enabled = false;
                Velocity = Vector2.zero;
                return;
            }
            
            base.HandleMovement();
        }
    }
}