using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerTopPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI cashText;
        public TextMeshProUGUI dayText;
        
        public Player.PlayerCombat playerCombat;
        public ShopManager shopManager;
        public TimeManager timeManager;

        void Update()
        {
            if (playerCombat)
                healthText.text = $"HP: {playerCombat.hp}";

            if (shopManager)
                cashText.text = $"Cash: {shopManager.playerCash}";

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