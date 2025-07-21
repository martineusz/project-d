using UI.Shop;
using UnityEngine;

namespace Environment.Buildable
{
    public class BuildablePlace : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;
        
        public ShopUI buildableUI;
        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }
        public void Use()
        {
            buildableUI.OpenShop();
        }
        
        public void Highlight()
        {
            if (_isHighlighted) return;
            spriteRenderer.color = Color.white;
            _isHighlighted = true;
        }

        public void DisableHighlight()
        {
            if (!_isHighlighted) return;
            spriteRenderer.color = _originalColor;
            _isHighlighted = false;
        }
    }
}