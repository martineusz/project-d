using System.Collections;
using UnityEngine;

namespace Environment.Gates
{
    public class SimpleLever : MonoBehaviour, INteractive
    {
        public SimpleGate gate;
        public float waitTime = 3f;

        private bool _isPulled = false;

        private float _lastUseTime = -Mathf.Infinity;

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