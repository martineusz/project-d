using System.Collections.Generic;
using Items;
using Player;
using UnityEngine;
using UI.Inventory;
using UnityEngine.Serialization;

namespace UI.Shop
{
    public class Shop : MonoBehaviour
    {
        public List<ItemData> itemsForSale = new List<ItemData>();
        public PlayerEnvironmentInteraction player;

        public void SellItem(ItemData item)
        {
            player.playerCash += item.price;
        }
        
        public bool BuyItem(ItemData item)
        {
            if (item == null || player.playerCash < item.price)
            {
                return false;
            }
            
            if (item.instantUse)
            {
                item.Use();
                player.playerCash -= item.price;
                itemsForSale.Remove(item);
                return true;
            }
            
            if (Inventory.Inventory.instance.Add(item))
            {
                player.playerCash -= item.price;
                itemsForSale.Remove(item);
                return true;
            }
            return false;
        }
    }
}