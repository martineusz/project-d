using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LightManipulation
{
    public class PlayerTorch : Torch
    {
        public Inventory inventory;
        public GameObject placableTorchPrefab;

        private void Update()
        {
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                ResetLight();
            }
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                PutTorch();
            }
        }
        
        public bool ResetLight()
        {
            if (!torchLight || inventory.torchesCount <= 0) return false;

            inventory.torchesCount--;
            lightModifier = 1f;
            torchLight.pointLightInnerRadius = InnerRadius;
            torchLight.pointLightOuterRadius = OuterRadius;
            torchTrigger.radius = InnerRadius;
            MaxForceRadius = InnerRadius + maxForceRadiusOffset;

            return true;
        }
        
        private void PutTorch()
        {
            if (!torchLight || inventory.torchesCount <= 0) return;

            var torch = Instantiate(placableTorchPrefab, transform.position, Quaternion.identity);
            torch.transform.SetParent(null);
            inventory.torchesCount--;
        }
    }
}