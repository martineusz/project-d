using System.Collections.Generic;
using Items;
using UnityEngine;

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
    }
}