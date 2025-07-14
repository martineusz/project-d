using System;
using Player;
using UnityEngine;

namespace Environment
{
    public class MudTerrain:MonoBehaviour
    {
        public float mudSpeedFactor = 0.68f;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemyBoid = other.GetComponent<Units.Boids.Enemies.EnemyBoid>();
                if (enemyBoid != null)
                {
                    enemyBoid.externalSpeedFactor *= mudSpeedFactor;
                }
                
            }
            if (other.CompareTag("Ally"))
            {
                var allyBoid = other.GetComponent<Units.Boids.Allies.AllyBoid>();
                if (allyBoid != null)
                {
                    allyBoid.externalSpeedFactor *= mudSpeedFactor;
                }
            }
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.externalSpeedFactor *= mudSpeedFactor;
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemyBoid = other.GetComponent<Units.Boids.Enemies.EnemyBoid>();
                if (enemyBoid != null)
                {
                    enemyBoid.externalSpeedFactor /= mudSpeedFactor;
                }
            }
            if (other.CompareTag("Ally"))
            {
                var allyBoid = other.GetComponent<Units.Boids.Allies.AllyBoid>();
                if (allyBoid != null)
                {
                    allyBoid.externalSpeedFactor /= mudSpeedFactor;
                }
            }
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.externalSpeedFactor /= mudSpeedFactor;
                }
            }
        }
    }
}