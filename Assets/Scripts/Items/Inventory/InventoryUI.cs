using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Items.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public Transform itemsParent;
        public GameObject inventoryUI;

        public GameObject itemDetailsPanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI rarityText;
        public Image itemImage;

        private Inventory _inventory;
        private InventorySlot[] _slots;

        void Start()
        {
            _inventory = Inventory.Instance;
            _inventory.OnItemChangedCallback += UpdateUI;
            _slots = itemsParent.GetComponentsInChildren<InventorySlot>();
            inventoryUI.SetActive(false);

            foreach (var slot in _slots)
            {
                slot.OnSlotClicked += ShowItemDetails;
            }
            itemDetailsPanel.SetActive(false);
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.iKey.wasPressedThisFrame)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
        }

        void UpdateUI()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (i < _inventory.items.Count)
                    _slots[i].AddItem(_inventory.items[i]);
                else
                    _slots[i].ClearSlot();
            }
        }

        void ShowItemDetails(ItemData item)
        {
            itemDetailsPanel.SetActive(true);
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            rarityText.text = item.rarity.ToString();
            itemImage.sprite = item.icon; 
            itemImage.enabled = item.icon != null;
        }
    }
}