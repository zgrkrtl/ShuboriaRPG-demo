using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private InputManager inputManager;

    private Rigidbody rb;
    private Animator animator;
    private Vector2 movementInput;
    private AbilityManager abilityManager;
    private CapsuleCollider playerCollider;

    private MandatoryData mandatoryData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mandatoryData = MandatoryDataSaveManager.Load();

        // Set initial position from saved data
        Vector3 savedPosition = mandatoryData.position.ToVector3();
        transform.position = savedPosition;
        rb.position = savedPosition;

        // Stop any residual motion
        rb.linearVelocity = Vector3.zero;

        animator = GetComponent<Animator>();
        abilityManager = GetComponent<AbilityManager>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        // Subscribe to movement input event
        if (inputManager != null)
            inputManager.OnMove += HandleMovementInput;
    }

    private void OnDisable()
    {
        // Unsubscribe from movement input event
        if (inputManager != null)
            inputManager.OnMove -= HandleMovementInput;
    }

    // Cache the current input for use in FixedUpdate
    private void HandleMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    private void FixedUpdate()
    {
        // Sync animation parameters with state
        animator.SetBool("IsAttacking", abilityManager.GetIsAttacking());
        animator.SetBool("IsMoving", movementInput != Vector2.zero);

        // Only allow movement if not attacking
        if (!abilityManager.GetIsAttacking())
        {
            Move();
        }
    }

    private void Update()
    {
        // Force gravity in case physics settings or constraints reduce natural fall
        rb.AddForce(Vector3.down * 100f, ForceMode.Acceleration);
    }

    private void Move()
    {
        if (movementInput == Vector2.zero) return;

        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        rb.linearVelocity = moveDirection * movementSpeed;

        // Smoothly rotate character toward movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 10f);
        }
    }

    // On death this is called as event
    public void ColliderDirectionToX()
    {
        playerCollider.direction = 0;
    }
}
