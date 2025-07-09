using System;
using Items;
using UnityEngine;

public class ShopLogic : MonoBehaviour
{
    public int playerCash = 1000;

    public void SellItem(ItemData item)
    {
        playerCash += item.price;
    }
}