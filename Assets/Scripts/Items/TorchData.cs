using UI.Inventory;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "newTorchItem", menuName = "Items/Torches", order = 0)]
    public class TorchData : ItemData
    {
        public int torchCount = 1;
        public override void Use()
        {
            Inventory inventory = Object.FindAnyObjectByType<Inventory>();

            if (inventory != null)
            {
                inventory.AddTorch(torchCount);
                Debug.Log($"Added {torchCount} torch(es) to inventory.");
            }
            else
            {
                Debug.LogWarning("No Inventory found in the scene.");
            }
        }
    }
}