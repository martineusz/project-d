using System.Collections.Generic;
using Items;
using UnityEngine;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        
        public int torchesLimit = 5;
        public int torchesCount = 2;

        public List<ItemData> items = new List<ItemData>();
        public int space = 20;

        public delegate void OnItemChanged();
        public OnItemChanged OnItemChangedCallback;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one inventory instance");
                return;
            }
            instance = this;
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