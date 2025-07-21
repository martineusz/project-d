using UnityEngine;

namespace Items
{
    public abstract class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;

        public string description;
        public Rarity rarity;
        public int price;
        
        public bool instantUse = false;
        public bool destroyShopPlaceOnUse = false;
        
        public Vector3 spawnPosition;

        public abstract void Use();
    }
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
