using UnityEngine;

namespace Items
{
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;

        public string description;
        public Rarity rarity;
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}