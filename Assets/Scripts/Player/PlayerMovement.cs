using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        private Vector2 _movement;
        private Rigidbody2D _rb;
    
        public BoidManager boidManager;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                boidManager.SelectNearestUnselectedAllyBoid();
            }
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                boidManager.DeselectFirstSelectedAllyBoid();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _movement * speed;
        }

    }
}