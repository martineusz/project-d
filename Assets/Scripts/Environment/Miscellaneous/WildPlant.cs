using Items;
using UI.Inventory;
using UnityEngine;

namespace Environment.Miscellaneous
{
    public class WildPlant : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;
        
        public ResourceDataFactory resourceDataFactory;

        private Crop _crop;

        public float growthEfficiency = 1f;

        [HideInInspector] public bool isHarvestable = true;
        private float _growthTimer = 0f;

        
        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }
        
        private void Start()
        {
            ResetCrop();
        }

        private void FixedUpdate()
        {
            _growthTimer += Time.fixedDeltaTime;
            if (_growthTimer >= 1f)
            {
                HandleGrowth();
                _growthTimer = 0f;
            }
        }


        private void HandleGrowth()
        {
            _crop.Grow(growthEfficiency);
            if (_crop.Maturity >= 1)
            {
                isHarvestable = true;
            }
        }

        public bool TryTakeResource(out ResourceData resource)
        {
            if (isHarvestable)
            {
                resource = _crop.Resource;
                ResetCrop();
                isHarvestable = false;
                return true;
            }

            resource = null;
            return false;
        }

        private void ResetCrop()
        {
            ResourceData newResData = resourceDataFactory.GenerateNewResource();
            _crop = new Crop(newResData);
        }
        
        public void Use()
        {
            if (TryTakeResource(out var resource))
            {
                Inventory.instance.Add(resource);
                Debug.Log("Resource taken from plant and added to inventory.");
            }
            else
            {
                Debug.Log("No resource available in plant.");
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