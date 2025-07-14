using TMPro;
using UI.Shop;
using UnityEngine;

namespace UI
{
    public class PlayerTopPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI cashText;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI torchText;
        
        public Player.PlayerCombat playerCombat;
        public Shop.Shop shop;
        public Inventory.Inventory inventory;
        public TimeManager timeManager;

        void Update()
        {
            if (playerCombat)
                healthText.text = $"HP: {playerCombat.hp}";

            if (shop)
                cashText.text = $"Cash: {shop.playerCash}";
            
            if (inventory)
                torchText.text = $"{inventory.torchesCount} / {inventory.torchesLimit} Torches";

            if (timeManager)
            {
                dayText.text = $"Day: {timeManager.day}";
                
                if (timeManager.currentDayType == TimeManager.DayType.BloodMoon)
                {
                    dayText.text += " (Blood Moon)";
                }
                else
                {
                    dayText.text += " (Normal)";
                }
            }
        }
    }
}