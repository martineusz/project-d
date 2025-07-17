using UI.Inventory;
using UnityEngine;

namespace Environment.Workplaces
{
    public class PlotStash : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private readonly float _alpha = 0.5f;
        private bool _isHighlighted;

        public WorkablePlot plot;

        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }

        public void Use()
        {
            if (!plot) return;
            if (plot.TryTakeResource(out var resource))
            {
                Inventory.instance.Add(resource);
                Debug.Log("Resource taken from plot and added to inventory.");
            }
            else
            {
                Debug.Log("No resources in plot stash.");
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
    }
}