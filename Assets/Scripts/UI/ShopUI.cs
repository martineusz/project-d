using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        public GameObject shopUIPanel;

        public void Start()
        {
            shopUIPanel.SetActive(false);
        }

        public void OpenShop()
        {
            if (UIManager.Instance.IsAnyUIOpen || !shopUIPanel) return;
            
            shopUIPanel.SetActive(true);
            UIManager.Instance.SetUIOpen(true);
        }

        void Update()
        {
            if (shopUIPanel.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseShop();
            }
        }

        private void CloseShop()
        {
            shopUIPanel.SetActive(false);
            UIManager.Instance.SetUIOpen(false);
        }
    }
}