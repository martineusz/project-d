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

        void Update()
        {
            if (playerCombat)
                healthText.text = $"HP: {playerCombat.hp}";

            if (shop)
                cashText.text = $"Cash: {shop.playerCash}";

            if (inventory)
                torchText.text = $"{inventory.torchesCount} / {inventory.torchesLimit} Torches";

            
            var timeManager = TimeManager.Instance;
            if (timeManager)
            {
                string dayTypeStr = timeManager.currentDayType == TimeManager.DayType.BloodMoon
                    ? "Blood Moon"
                    : "Normal";

                dayText.text = $"Day: {timeManager.day} ({dayTypeStr})";
            }
        }
    }
}