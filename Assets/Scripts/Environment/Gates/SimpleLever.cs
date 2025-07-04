using System.Collections;
using UnityEngine;

namespace Environment.Gates
{
    public class SimpleLever : MonoBehaviour, INteractive
    {
        public SimpleGate gate;
        public float waitTime = 3f;

        public void Use()
        {
            StartCoroutine(OpenGateWithDelay());
        }

        private IEnumerator OpenGateWithDelay()
        {
            yield return new WaitForSeconds(waitTime);
            if (gate)
            {
                gate.OpenGate();
            }
        }
    }
}