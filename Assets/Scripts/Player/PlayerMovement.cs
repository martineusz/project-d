using Units.Boids;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float _speed;
        public float runSpeed = 5f;
        public float walkSpeed = 3.5f;
        private Vector2 _movement;
        private Rigidbody2D _rb;
        [HideInInspector] public float externalSpeedFactor = 1f;

        public BoidManager boidManager;

        private void Awake()
        {
            _speed = runSpeed;
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
            if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                boidManager.SelectNearestUnselectedWorkerBoid();
            }

            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                boidManager.DeselectFirstSelectedWorkerBoid();
            }
            

            if (Keyboard.current.leftShiftKey.isPressed)
            {
                _speed = walkSpeed;
            }
            else
            {
                _speed = runSpeed;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _movement * (_speed * externalSpeedFactor);
        }
    }
}