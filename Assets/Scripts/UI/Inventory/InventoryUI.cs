using Items;
using TMPro;
using UI.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public Transform itemsParent;
        public GameObject inventoryUI;

        public GameObject itemDetailsPanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI rarityText;
        public TextMeshProUGUI priceText;
        public Image itemImage;
        public Button sellButton; 
        public Shop.Shop shop; 

        private UI.Inventory.Inventory _inventory;
        private InventorySlot[] _slots;
        private ItemData _currentItem;

        void Start()
        {
            _inventory = UI.Inventory.Inventory.Instance;
            _inventory.OnItemChangedCallback += UpdateUI;
            _slots = itemsParent.GetComponentsInChildren<InventorySlot>();
            inventoryUI.SetActive(false);
            itemDetailsPanel.SetActive(false);

            foreach (var slot in _slots)
            {
                slot.OnSlotClicked += ShowItemDetails;
            }

            sellButton.onClick.AddListener(OnSellButtonClicked); // Assign click event
            sellButton.gameObject.SetActive(false);
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.iKey.wasPressedThisFrame)
            {
                HandleInventoryToggle();
            }
        }
        
        void HandleInventoryToggle()
        {
            if (!inventoryUI.activeSelf && UIManager.Instance.IsAnyUIOpen)
                return;

            bool newState = !inventoryUI.activeSelf;
            inventoryUI.SetActive(newState);
            itemDetailsPanel.SetActive(newState);
            UIManager.Instance.SetUIOpen(newState);
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
            _currentItem = item;
            itemDetailsPanel.SetActive(true);
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            rarityText.text = item.rarity.ToString();
            priceText.text = item.price.ToString();
            itemImage.sprite = item.icon;
            itemImage.enabled = item.icon != null;
            sellButton.gameObject.SetActive(true);
        }

        void OnSellButtonClicked()
        {
            if (_currentItem != null && shop != null)
            {
                shop.SellItem(_currentItem);
                _inventory.Remove(_currentItem);
                itemDetailsPanel.SetActive(false);
            }
        }
    }
}