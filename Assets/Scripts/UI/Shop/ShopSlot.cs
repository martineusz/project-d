using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopSlot : MonoBehaviour, IPointerClickHandler
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