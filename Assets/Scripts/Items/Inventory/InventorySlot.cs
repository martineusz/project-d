using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Items.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public Image icon;
        public ItemData item;

        public delegate void SlotClicked(ItemData item);
        public event SlotClicked OnSlotClicked;

        public void AddItem(ItemData newItem)
        {
            item = newItem;
            icon.sprite = newItem.icon;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (item != null)
                OnSlotClicked?.Invoke(item);
        }
    }
}