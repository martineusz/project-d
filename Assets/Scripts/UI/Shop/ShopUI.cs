using Items;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        public GameObject shopUIPanel;

        public Transform itemsParent;
        public GameObject itemDetailsPanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI rarityText;
        public TextMeshProUGUI priceText;
        public Image itemImage;
        public Button buyButton;
        public Shop shop;

        private ShopSlot[] _slots;
        private ItemData _currentItem;

        public void Start()
        {
            shopUIPanel.SetActive(false);

            _slots = itemsParent.GetComponentsInChildren<ShopSlot>();
            itemDetailsPanel.SetActive(false);

            foreach (var slot in _slots)
            {
                slot.OnSlotClicked += ShowItemDetails;
            }

            buyButton.onClick.AddListener(OnBuyButtonClicked);
            buyButton.gameObject.SetActive(false);
        }

        void PopulateShopSlots()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (shop && i < shop.itemsForSale.Count)
                    _slots[i].AddItem(shop.itemsForSale[i]);
                else
                    _slots[i].ClearSlot();
            }
        }

        public void OpenShop()
        {
            if (UIManager.Instance.IsAnyUIOpen || !shopUIPanel) return;
            
            Debug.Log("Opening shop UI");
            PopulateShopSlots();
            shopUIPanel.SetActive(true);
            UIManager.Instance.SetUIOpen(true);
        }

        void Update()
        {
            if (shopUIPanel.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseShop();
            }
        }

        private void CloseShop()
        {
            shopUIPanel.SetActive(false);
            UIManager.Instance.SetUIOpen(false);
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
            buyButton.gameObject.SetActive(true);
        }

        void OnBuyButtonClicked()
        {
            if (_currentItem != null && shop != null)
            {
                bool isBought = shop.BuyItem(_currentItem);
                if (isBought)
                {
                    _currentItem = null;
                    itemDetailsPanel.SetActive(false);
                    PopulateShopSlots(); // Refresh the shop slots
                }
                else
                {
                    Debug.Log("Not enough cash to buy the item.");
                }
            }
        }
    }
}