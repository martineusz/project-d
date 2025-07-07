using UnityEngine;
using UnityEngine.InputSystem;

namespace Items.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public Transform itemsParent;
        public GameObject inventoryUI;

        private Inventory _inventory;
        private InventorySlot[] _slots;

        void Start()
        {
            _inventory = Inventory.Instance;
            _inventory.OnItemChangedCallback += UpdateUI;
            _slots = itemsParent.GetComponentsInChildren<InventorySlot>();
            inventoryUI.SetActive(false);
        }
        
        void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
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
    }
}