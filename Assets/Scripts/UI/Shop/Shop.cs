using System.Collections.Generic;
using Items;
using UnityEngine;
using UI.Inventory;

namespace UI.Shop
{
    public class Shop : MonoBehaviour
    {
        public int playerCash = 1000;
        public List<ItemData> itemsForSale = new List<ItemData>();


        public void SellItem(ItemData item)
        {
            playerCash += item.price;
        }
        
        public bool BuyItem(ItemData item)
        {
            if (item == null || playerCash < item.price)
            {
                return false;
            }
            
            if (item.instantUse)
            {
                item.Use();
                playerCash -= item.price;
                return true;
            }
            
            if (Inventory.Inventory.instance.Add(item))
            {
                playerCash -= item.price;
                itemsForSale.Remove(item);
                return true;
            }
            return false;
        }
    }
}