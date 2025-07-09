using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerTopPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI cashText;
        public Player.PlayerCombat playerCombat;
        public ShopLogic shopLogic;

        void Update()
        {
            if (playerCombat)
                healthText.text = $"HP: {playerCombat.hp}";

            if (shopLogic)
                cashText.text = $"Cash: {shopLogic.playerCash}";
        }
    }
}