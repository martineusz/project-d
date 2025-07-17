using System.Collections.Generic;
using Items;
using UI.Inventory;
using UnityEngine;

namespace Environment.Miscellaneous
{
    public class TreasureChest : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;

        public List<ItemData> treasureItems = new List<ItemData>();

        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }

        public void Use()
        {
            if (TryTakeResource(out var resource))
            {
                Inventory.instance.Add(resource);
                Debug.Log("Item taken from treasure and added to inventory.");
            }
            else
            {
                Destroy(gameObject);
            }
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

        private bool TryTakeResource(out ItemData resource)
        {
            if (treasureItems.Count > 0)
            {
                resource = treasureItems[0];
                treasureItems.RemoveAt(0);
                return true;
            }

            resource = null;
            return false;
        }
    }
}