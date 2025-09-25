using UnityEngine;
using UnityEngine.InputSystem;

namespace EclipseProtocol.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 7.5f;
        [SerializeField] private float acceleration = 12f;
        [SerializeField] private float deceleration = 16f;
        [SerializeField] private float gravity = -24f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float airControl = 0.35f;
        [SerializeField] private float slideSpeed = 9f;
        [SerializeField] private float slideDuration = 0.75f;

        [Header("View")]
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private float lookSensitivity = 0.8f;
        [SerializeField] private float lookSmoothing = 12f;
        [SerializeField] private Vector2 lookLimits = new Vector2(-85f, 85f);

        [Header("Audio")]
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioClip[] footstepClips;

        private CharacterController controller;
        private PlayerInput input;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 velocity;
        private float currentSpeed;
        private bool isSprinting;
        private bool isSliding;
        private float slideTimer;
        private float cameraPitch;
        private float targetSpeed;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private Animator animator;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();
            animator = GetComponentInChildren<Animator>();

            if (cameraRoot == null)
            {
                cameraRoot = GetComponentInChildren<Camera>()?.transform;
            }
        }

        private void OnEnable()
        {
            input.actions["Move"].performed += OnMove;
            input.actions["Move"].canceled += OnMove;
            input.actions["Look"].performed += OnLook;
            input.actions["Jump"].performed += OnJump;
            input.actions["Sprint"].performed += OnSprintStart;
            input.actions["Sprint"].canceled += OnSprintEnd;
            input.actions["Slide"].performed += OnSlide;
        }

        private void OnDisable()
        {
            input.actions["Move"].performed -= OnMove;
            input.actions["Move"].canceled -= OnMove;
            input.actions["Look"].performed -= OnLook;
            input.actions["Jump"].performed -= OnJump;
            input.actions["Sprint"].performed -= OnSprintStart;
            input.actions["Sprint"].canceled -= OnSprintEnd;
            input.actions["Slide"].performed -= OnSlide;
        }

        private void Update()
        {
            HandleMovement(Time.deltaTime);
            HandleLook(Time.deltaTime);
            HandleSlide(Time.deltaTime);
            UpdateAnimator();
        }

        private void HandleMovement(float deltaTime)
        {
            var desiredDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            desiredDirection = transform.TransformDirection(desiredDirection);

            float speed = isSprinting ? sprintSpeed : walkSpeed;
            targetSpeed = desiredDirection.sqrMagnitude > 0.01f ? speed : 0f;

            float accel = desiredDirection.sqrMagnitude > 0.01f ? acceleration : deceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * deltaTime);

            Vector3 horizontalVelocity = desiredDirection.normalized * currentSpeed;

            if (controller.isGrounded)
            {
                velocity.y = -1f; // Keeps player grounded
                if (input.actions["Jump"].WasPressedThisFrame())
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            else
            {
                velocity.y += gravity * deltaTime;
                horizontalVelocity = Vector3.Lerp(controller.velocity, horizontalVelocity, airControl);
            }

            Vector3 motion = (horizontalVelocity + Vector3.up * velocity.y) * deltaTime;
            controller.Move(motion);

            if (controller.isGrounded && horizontalVelocity.magnitude > 0.1f)
            {
                PlayFootstep();
            }
        }

        private void HandleLook(float deltaTime)
        {
            float yaw = lookInput.x * lookSensitivity;
            float pitch = lookInput.y * lookSensitivity;

            cameraPitch = Mathf.Clamp(cameraPitch - pitch, lookLimits.x, lookLimits.y);
            Quaternion targetCameraRot = Quaternion.Euler(cameraPitch, 0f, 0f);
            cameraRoot.localRotation = Quaternion.Slerp(cameraRoot.localRotation, targetCameraRot, deltaTime * lookSmoothing);

            transform.Rotate(Vector3.up * yaw);
        }

        private void HandleSlide(float deltaTime)
        {
            if (!isSliding)
            {
                return;
            }

            slideTimer += deltaTime;
            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                controller.height = 1.8f;
                controller.center = new Vector3(0f, controller.height / 2f, 0f);
            }
            else
            {
                Vector3 slideDirection = new Vector3(moveInput.x, 0f, moveInput.y);
                if (slideDirection.sqrMagnitude < 0.01f)
                {
                    slideDirection = transform.forward;
                }

                controller.Move(slideDirection.normalized * slideSpeed * deltaTime);
            }
        }

        private void UpdateAnimator()
        {
            if (animator == null)
            {
                return;
            }

            float planarSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
            animator.SetFloat(SpeedHash, planarSpeed / sprintSpeed, 0.1f, Time.deltaTime);
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            // Jump handled in Update via WasPressedThisFrame
        }

        private void OnSprintStart(InputAction.CallbackContext context)
        {
            isSprinting = context.ReadValueAsButton();
        }

        private void OnSprintEnd(InputAction.CallbackContext context)
        {
            isSprinting = false;
        }

        private void OnSlide(InputAction.CallbackContext context)
        {
            if (!controller.isGrounded)
            {
                return;
            }

            isSliding = true;
            slideTimer = 0f;
            controller.height = 1.2f;
            controller.center = new Vector3(0f, controller.height / 2f, 0f);
        }

        private void PlayFootstep()
        {
            if (footstepSource == null || footstepClips == null || footstepClips.Length == 0)
            {
                return;
            }

            if (!footstepSource.isPlaying)
            {
                int index = Random.Range(0, footstepClips.Length);
                footstepSource.clip = footstepClips[index];
                footstepSource.Play();
            }
        }
    }
}
