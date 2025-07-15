using Units.Combat.Enemies;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightManipulation
{
    public class Torch : MonoBehaviour
    {
        public Light2D torchLight;
        public CircleCollider2D torchTrigger;

        public float lightDiminishFrequency = 1f;
        public float lightDiminishSpeed = 0.001f;
        [HideInInspector] public float lightModifier = 1f;
        protected float InnerRadius;
        protected float OuterRadius;

        private float _lastDiminishTime = -Mathf.Infinity;

        public float maxForce = 20f;
        public float maxForceRadiusOffset = 5f;
        protected float MaxForceRadius;

        private void Start()
        {
            InnerRadius = torchLight.pointLightInnerRadius;
            OuterRadius = torchLight.pointLightOuterRadius;

            torchTrigger.radius = InnerRadius;
            MaxForceRadius = InnerRadius + maxForceRadiusOffset;

            if (torchLight == null)
            {
                torchLight = GetComponent<Light2D>();
            }
        }

        private void FixedUpdate()
        {
            DiminishLight();
        }

        private void DiminishLight()
        {
            if (!torchLight) return;
            if (lightModifier <= 0f) return;
            if ((Time.time - _lastDiminishTime < 1f / lightDiminishFrequency)) return;


            lightModifier -= lightDiminishSpeed;
            torchLight.pointLightInnerRadius = InnerRadius * lightModifier;
            torchLight.pointLightOuterRadius = OuterRadius * lightModifier;
            torchTrigger.radius = torchLight.pointLightInnerRadius;
            MaxForceRadius = torchLight.pointLightInnerRadius + maxForceRadiusOffset;

            _lastDiminishTime = Time.time;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyCombat enemyCombat = collision.GetComponent<EnemyCombat>();
                Rigidbody2D enemyRb = collision.attachedRigidbody;
                if (enemyRb == null) return;

                Vector2 direction = (collision.transform.position - transform.position);
                float distance = direction.magnitude;
                Vector2 forceDirection = direction.normalized;

                // lightModifier and fearOfLightModifier are used to scale the force applied to the enemy
                float forceAmount = Mathf.Clamp01(1 - (distance / MaxForceRadius)) * maxForce *
                                    (1 / enemyCombat.fearOfLightModifier) * lightModifier;
                enemyRb.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);
            }
        }
    }
}