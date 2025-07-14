using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace LightManipulation
{
    public class PlayerTorch : MonoBehaviour
    {
        public Light2D torchLight;

        public float lightDiminishFrequency = 1f;
        public float lightDiminishSpeed = 0.001f;
        [HideInInspector] public float lightModifier = 1f;
        private float _innerRadius;
        private float _outerRadius;

        private float _lastDiminishTime = -Mathf.Infinity;

        private void Start()
        {
            _innerRadius = torchLight.pointLightInnerRadius;
            _outerRadius = torchLight.pointLightOuterRadius;

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

                _lastDiminishTime = Time.time;
            }
        }

        public bool ResetLight()
        {
            if (!torchLight) return false;
            
            lightModifier = 1f;
            torchLight.pointLightInnerRadius = _innerRadius;
            torchLight.pointLightOuterRadius = _outerRadius;
            
            return true;

        }
    }
}