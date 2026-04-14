using UnityEngine;
using UnityEngine.InputSystem;

namespace YesChef.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 7f;

        [SerializeField] private float rotationSpeed = 15f;

        [Header("Input Actions")] [SerializeField]
        private InputActionReference moveAction;

        private CharacterController characterController;
        private Vector3 movementVector;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
        }

        private void HandleMovement()
        {
            // Read input from Unity's modern Input System
            var input = moveAction.action.ReadValue<Vector2>();
            movementVector = new Vector3(input.x, 0f, input.y).normalized;

            // Apply movement
            characterController.Move(movementVector * (moveSpeed * Time.deltaTime));

            // Handle Rotation to face movement direction
            if (movementVector != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(movementVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Keep player grounded simply
            if (!characterController.isGrounded)
            {
                characterController.Move(Vector3.down * (9.81f * Time.deltaTime));
            }
        }
    }
}