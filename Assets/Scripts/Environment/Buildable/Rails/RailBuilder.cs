using UnityEngine;

namespace Environment.Buildable.Rails
{
    public class RailBuilder : MonoBehaviour, INteractive
    {
        public GameObject railPrefab;
        
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;
        
        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }
        
        public void Use()
        {
            railPrefab.SetActive(true);
            gameObject.SetActive(false);
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