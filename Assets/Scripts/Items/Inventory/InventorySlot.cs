using UnityEngine;
using UnityEngine.UI;

namespace Items.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon;

        public void AddItem(ItemData newItem)
        {
            icon.sprite = newItem.icon;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}