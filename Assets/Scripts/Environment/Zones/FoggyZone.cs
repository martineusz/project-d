using LightManipulation;
using UnityEngine;

namespace Environment.Zones
{
    public class FoggyZone : MonoBehaviour
    {   
        public float fogDensity = 0.5f;

        private void Start()
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = fogDensity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Torch torch = other.GetComponentInChildren<Torch>();
                if (torch != null)
                {
                    torch.lightEnvironmentModifier *= fogDensity;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Torch torch = other.GetComponentInChildren<Torch>();
                if (torch != null)
                {
                    torch.lightEnvironmentModifier /= fogDensity;
                }
            }
        }
    }
}