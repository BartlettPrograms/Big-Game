using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class BallPlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField] 
    private float playerTorque = 10;
    [SerializeField] 
    private float jumpForce = 100.0f;
    [SerializeField]
    private LayerMask platformLayerMask;
    [SerializeField] 
    private float fallMultiplier = 3f;
    [SerializeField] 
    private float lowJumpMultiplier = 1.5f;
    [SerializeField] 
    private float stopVelocity = 2f;
    [SerializeField] 
    private float drag = 5f;
    [SerializeField]
    private float dashForce = 20f;
    [SerializeField]
    private float playerMaxAngularVelocity = 35f;

    [SerializeField] private float playerSpeed = 2f;
    
    protected bool groundedPlayer;
    protected PlayerInput playerInput;
    protected bool groundedSinceDash = true;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        Cursor.lockState = CursorLockMode.Locked;
        rb.maxAngularVelocity = playerMaxAngularVelocity;
        
    }

    void Update()
    {
        // do WASD player movement
        rolling();
        jumping();
        falling();
        unStagnate();
        dashAction.performed += _ => dash();
        dashAction.performed -= _ => dash();
    }

    // Jumping functions
    private void OnTriggerStay(Collider collider)
    {
        groundedPlayer = collider != null && (((1 << collider.gameObject.layer) & platformLayerMask) != 0);
        groundedSinceDash = groundedPlayer;
    }
    private void OnTriggerExit(Collider collision)
    {
        groundedPlayer = false;
    }
    // Jumping functions end
    
    private void rolling()
    {
        // get WASD player movement
        Vector2 input = moveAction.ReadValue<Vector2>();
        input.x *= -1;
        Vector3 moveT = new Vector3(input.x, 0, input.y);
        Vector3 moveF = new Vector3(input.x, 0, input.y);
        moveT = moveT.x * cameraTransform.forward.normalized + moveT.z * cameraTransform.right.normalized;
        moveF = moveF.x * cameraTransform.right.normalized * -1f + moveT.z * cameraTransform.forward.normalized * -1f;
        moveT.y = 0; // fix calc for 2d input
        rb.AddTorque(moveT * playerTorque);
        rb.AddForce(moveF * playerSpeed, ForceMode.Force);
    }

    private void jumping()
    {
        if (jumpAction.triggered && groundedPlayer)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void falling()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 &&  !jumpAction.triggered)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void unStagnate()
    {
        if (rb.velocity.magnitude < stopVelocity)
        {
            rb.angularDrag = drag;
        }
        else
        {
            rb.angularDrag = 0;
        }
    }

    private void dash()
    {
        if (groundedSinceDash)
        {
            Vector3 dashDirection = cameraTransform.forward.normalized;
            dashDirection.y = 0;
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            groundedSinceDash = false;
        }
    }
}
