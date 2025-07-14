using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightManipulation
{
    public class PlayerTorch : MonoBehaviour
    {
        public Light2D light;
        
        public float lightDiminishSpeed = 0.05f;
        [HideInInspector] public float lightModifier = 1f;
        private float innerRadius = 7.5f;
        private float outerRadius = 5f;
        
        private float _lastDiminishTime = -Mathf.Infinity;
        
        private void Start()
        {
            if (light == null)
            {
                light = GetComponent<Light2D>();
            }
        }

        private void FixedUpdate()
        {
            DiminishLight();
        }
        
        private void DiminishLight()
        {
            if (light)
            {
                if ((Time.time - _lastDiminishTime < 1f)) return;
                if (lightModifier <= 0f) return;
                
                lightModifier -= lightDiminishSpeed;
                light.pointLightInnerRadius = innerRadius * lightModifier;
                light.pointLightOuterRadius = outerRadius * lightModifier;
                
                _lastDiminishTime = Time.time;
            }
        }
    }
}