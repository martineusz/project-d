using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace LightManipulation
{
    public class PlayerTorch : MonoBehaviour
    {
        public Light2D torchLight;
        public Inventory inventory;
        public CircleCollider2D torchTrigger;

        public float lightDiminishFrequency = 1f;
        public float lightDiminishSpeed = 0.001f;
        [HideInInspector] public float lightModifier = 1f;
        private float _innerRadius;
        private float _outerRadius;

        private float _lastDiminishTime = -Mathf.Infinity;
        
        public float maxForce = 20f;
        public float maxForceRadiusOffset = 5f;
        private float _maxForceRadius;

        private void Start()
        {
            _innerRadius = torchLight.pointLightInnerRadius;
            _outerRadius = torchLight.pointLightOuterRadius;
            
            torchTrigger.radius = _innerRadius;
            _maxForceRadius = _innerRadius + maxForceRadiusOffset;

            if (torchLight == null)
            {
                torchLight = GetComponent<Light2D>();
            }
        }

        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                ResetLight();
            }
        }

        private void FixedUpdate()
        {
            DiminishLight();
        }

        private void DiminishLight()
        {
            if (torchLight)
            {
                if ((Time.time - _lastDiminishTime < 1f / lightDiminishFrequency)) return;
                if (lightModifier <= 0f) return;

                lightModifier -= lightDiminishSpeed;
                torchLight.pointLightInnerRadius = _innerRadius * lightModifier;
                torchLight.pointLightOuterRadius = _outerRadius * lightModifier;
                torchTrigger.radius = torchLight.pointLightInnerRadius;
                _maxForceRadius = torchLight.pointLightInnerRadius + maxForceRadiusOffset;

                _lastDiminishTime = Time.time;
            }
        }

        public bool ResetLight()
        {
            if (!torchLight || inventory.torchesCount <= 0) return false;
            
            inventory.torchesCount--;
            lightModifier = 1f;
            torchLight.pointLightInnerRadius = _innerRadius;
            torchLight.pointLightOuterRadius = _outerRadius;
            torchTrigger.radius = _innerRadius;
            _maxForceRadius = _innerRadius + maxForceRadiusOffset;
            
            return true;

        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Rigidbody2D enemyRb = collision.attachedRigidbody;
                if (enemyRb == null) return;
                
                Vector2 direction = (collision.transform.position - transform.position);
                float distance = direction.magnitude;
                Vector2 forceDirection = direction.normalized;

                float forceAmount = Mathf.Clamp01(1 - (distance / _maxForceRadius)) * maxForce;
                enemyRb.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);
            }
        }
    }
}