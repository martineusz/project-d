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
        
        [Header("Stamina Settings")]
        public float maxStamina = 100f;
        public float staminaDrainRate = 15f;
        public float staminaRegenRate = 10f;
        public float minStaminaToRun = 5f;
        
        private float staminaCooldownTimer = 0f;
        public float staminaCooldownDuration = 2f;
        
        private float currentStamina;
        private bool isRunning;

        public BoidManager boidManager;

        private void Awake()
        {
            _speed = runSpeed;
            _rb = GetComponent<Rigidbody2D>();
            currentStamina = maxStamina;
        }

        private void Update()
        {
            // --- Boid selection controls ---
            if (Keyboard.current.eKey.wasPressedThisFrame)
                boidManager.SelectNearestUnselectedAllyBoid();

            if (Keyboard.current.qKey.wasPressedThisFrame)
                boidManager.DeselectFirstSelectedAllyBoid();

            if (Keyboard.current.xKey.wasPressedThisFrame)
                boidManager.SelectNearestUnselectedWorkerBoid();

            if (Keyboard.current.zKey.wasPressedThisFrame)
                boidManager.DeselectFirstSelectedWorkerBoid();

            // --- Stamina-controlled running ---
            bool shiftHeld = Keyboard.current.leftShiftKey.isPressed;

            if (staminaCooldownTimer > 0f)
            {
                staminaCooldownTimer -= Time.deltaTime;
            }

            if (shiftHeld && currentStamina > minStaminaToRun && staminaCooldownTimer <= 0f)
            {
                isRunning = true;
                _speed = runSpeed;
                currentStamina -= staminaDrainRate * Time.deltaTime;
                currentStamina = Mathf.Max(currentStamina, 0f);

                if (currentStamina <= minStaminaToRun)
                {
                    staminaCooldownTimer = staminaCooldownDuration;
                }
            }
            else
            {
                isRunning = false;
                _speed = walkSpeed;
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
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