using UnityEngine;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/


/// <summary>
/// Main script for third-person movement of the character in the game.
/// Make sure that the object that will receive this script (the player) 
/// has the Player tag and the Character Controller component.
/// </summary>
public class ThirdPersonController : MonoBehaviour
{
    [HideInInspector] public bool live = true;

    public FixedJoystick joystick;
    public float speed = 50f;
    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Player states
    bool isJumping = false;
    bool isSprinting = false;
    bool isCrouching = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputJump;
    bool inputCrouch;
    bool inputSprint;

    Animator animator;
    CharacterController cc;
    private Vector3 ccCenter;

    private Vector3 movement;
    private Vector3 movementWithoutGravity;
    private Vector3 iceMovement;
    // Lifts
    int liftId;
    Vector3 direction1 = new Vector3(0, 0, 0);
    Vector3 direction2 = new Vector3(0, 0, 0);

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Message informing the user that they forgot to add an animator
        if (animator == null)
            Debug.LogWarning("Hey buddy, you don't have the Animator component in your player. Without it, the animations won't work.");
    }


    // Update is only being used here to identify keys and trigger animations
    void Update()
    {

        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputJump = Input.GetAxis("Jump") == 1f;

        if (cc.isGrounded)
        {
            // Run
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed);
        }

        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
        }


    }

    public void MobileJump()
    {
        if (cc.isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        ccCenter = transform.TransformPoint(cc.center);
        HeadHittingDetect();

        float velocityAdittion = 0;
        if (isSprinting)
            velocityAdittion = sprintAdittion;
        if (isCrouching)
            velocityAdittion = -(velocity * 0.50f); // -50% velocity

        // Direction movement

        float directionX;
        float directionZ;
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            directionX = joystick.Horizontal * (velocity + velocityAdittion) * Time.deltaTime;
            directionZ = joystick.Vertical * (velocity + velocityAdittion) * Time.deltaTime;
        }
        else
        {
            directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
            directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        }
        float directionY = 0;

        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Jump timer
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;


        // --- Character rotation --- 

        Vector3 forward = -Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(-forward.x + right.x, -forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle - 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // --- End rotation ---


        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = -forward + right;

        movementWithoutGravity = horizontalDirection;
        movement = verticalDirection + horizontalDirection;

        OnLift();
        if (!OnIce())
        {
            if (live)
            {
                cc.Move(movement);
            }
        }
    }

    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        // Uncomment this line to see the Ray drawed in your characters head
        // Debug.DrawRay(ccCenter, Vector3.up * headHeight, Color.red);

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

    void OnLift()
    {
        if (Physics.Raycast(ccCenter, Vector3.down, out RaycastHit hitInfo, cc.height / 2f + 1f, LayerMask.GetMask("Lift")))
        {
            LiftScript script = hitInfo.transform.GetComponent<LiftScript>();
            if (liftId != hitInfo.transform.gameObject.GetInstanceID())
            {
                Debug.Log(hitInfo.transform.gameObject.GetInstanceID());
                liftId = hitInfo.transform.gameObject.GetInstanceID();
                direction1 = script.Direction1;
                direction2 = script.Direction2;
            }

            if (!live) { return; }
             if (script.stayOn1)
            {
                cc.Move(Time.deltaTime * script.speed * -script.Direction1);
            }
            else if (script.stayOn2)
            {
                cc.Move(Time.deltaTime * script.speed * -script.Direction2);
            }

        }
    }

    bool OnIce()
    {
        if (Physics.Raycast(ccCenter, Vector3.down, out RaycastHit hitInfo, cc.height / 2f + 0.1f, LayerMask.GetMask("Ice")))
        {
            if (Mathf.Abs(movementWithoutGravity.magnitude) >= 0.05)
            {
                iceMovement = movementWithoutGravity;
            }

            if (!live) { return true; }
            cc.Move(iceMovement);
            return true;
        }
        return false;
    }
}
