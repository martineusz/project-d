using System.Collections.Generic;
using Items;
using UnityEngine;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        public List<ItemData> items = new List<ItemData>();
        public int space = 20;

        public delegate void OnItemChanged();
        public OnItemChanged OnItemChangedCallback;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one inventory instance");
                return;
            }
            Instance = this;
        }

        public bool Add(ItemData item)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough space.");
                return false;
            }

            items.Add(item);
            OnItemChangedCallback?.Invoke();
            return true;
        }

        public void Remove(ItemData item)
        {
            items.Remove(item);
            OnItemChangedCallback?.Invoke();
        }
    }
}