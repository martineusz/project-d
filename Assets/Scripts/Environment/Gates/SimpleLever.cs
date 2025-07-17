using System.Collections;
using UnityEngine;

namespace Environment.Gates
{
    public class SimpleLever : MonoBehaviour, INteractive
    {
        public SpriteRenderer spriteRenderer;
        private Color _originalColor;
        private bool _isHighlighted;
        
        public SimpleGate gate;
        public float waitTime = 3f;

        private bool _isPulled = false;

        private float _lastUseTime = -Mathf.Infinity;

        
        private void Awake()
        {
            _originalColor = spriteRenderer.color;
        }
        
        public void Use()
        {
            if (Time.time - _lastUseTime < waitTime) return;

            if (_isPulled)
            {
                StartCoroutine(CloseGateWithDelay());
                _isPulled = false;
            }
            else
            {
                StartCoroutine(OpenGateWithDelay());
                _isPulled = true;
            }

            _lastUseTime = Time.time; 
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

        private IEnumerator OpenGateWithDelay()
        {
            yield return new WaitForSeconds(waitTime);
            if (gate)
            {
                gate.OpenGate();
            }
        }

        private IEnumerator CloseGateWithDelay()
        {
            yield return new WaitForSeconds(waitTime);
            if (gate)
            {
                gate.gameObject.SetActive(true);
            }
        }
    }
}