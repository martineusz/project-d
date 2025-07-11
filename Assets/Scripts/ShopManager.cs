using System;
using Items;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int playerCash = 1000;

    public void SellItem(ItemData item)
    {
        playerCash += item.price;
    }
}