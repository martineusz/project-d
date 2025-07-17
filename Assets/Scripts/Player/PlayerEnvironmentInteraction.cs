using System.Collections.Generic;
using Environment;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerEnvironmentInteraction : MonoBehaviour
    {
        private List<INteractive> _interactives = new List<INteractive>();
        private INteractive _closestInteractive;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Interactive"))
            {
                _interactives.Add(other.gameObject.GetComponent<INteractive>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Interactive"))
            {
                var interactive = other.gameObject.GetComponent<INteractive>();
                if (interactive != null)
                {
                    _interactives.Remove(interactive);
                }
            }
        }

        private void Update()
        {
            FindClosestInteractive();
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                HandleInteraction();
            }
        }

        private void HandleInteraction()
        {
            if (_closestInteractive == null)
            {
                Debug.Log("No closest interactive found");
                return;
            }

            Debug.Log("Using interactive: " + ((MonoBehaviour)_closestInteractive).gameObject.name);
            _closestInteractive.Use();
        }

        private void FindClosestInteractive()
        {
            float minDist = float.MaxValue;
            
            INteractive currentClosestInteractive = null;
            Vector3 playerPos = transform.position;

            foreach (var interactive in _interactives)
            {
                if (interactive == null) continue;
                float dist = Vector3.Distance(playerPos, ((MonoBehaviour)interactive).transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    currentClosestInteractive = interactive;
                }
            }
            if (currentClosestInteractive == null)
            {
                if (_closestInteractive != null)
                {
                    Debug.LogWarning("Disabling highlight");
                    _closestInteractive.DisableHighlight();
                    _closestInteractive = null;
                }
                return;
            }
            if (currentClosestInteractive != _closestInteractive)
            {
                if (_closestInteractive != null)
                {
                    Debug.LogWarning("Disabling highlight");
                    _closestInteractive.DisableHighlight();
                }
                _closestInteractive = currentClosestInteractive;
                Debug.LogWarning("Highigihgh gigih");
                _closestInteractive.Highlight();
            }
        }
    }
}