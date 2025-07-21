using Environment;
using UI;
using UI.Shop;
using UnityEngine;

namespace Shop
{
    public class ShopPlace : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;
        
        public ShopUI shopUI;

        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }
        public void Use()
        {
            shopUI.OpenShop(gameObject);
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