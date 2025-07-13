using System.Collections.Generic;
using Items;
using UI.Inventory;
using UnityEngine;

namespace Environment.Miscellaneous
{
    public class TreasureChest : MonoBehaviour, INteractive
    {
        public List<ItemData> treasureItems = new List<ItemData>();
        
        public void Use()
        {
            if (TryTakeResource(out var resource))
            {
                Inventory.instance.Add(resource);
                Debug.Log("Item taken from treasure and added to inventory.");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private bool TryTakeResource(out ItemData resource)
        {
            if (treasureItems.Count > 0)
            {
                resource = treasureItems[0];
                treasureItems.RemoveAt(0);
                return true;
            }

            resource = null;
            return false;
        }
    }
}