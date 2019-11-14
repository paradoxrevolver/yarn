using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    /* VISIBLE IN INSPECTOR */
    [Header("Values")]
    [Tooltip("Modifies the base movement speed of the player.")]
    [SerializeField] private float moveSpeed = 300f;
    
    /* NOT VISIBLE IN INSPECTOR */
    private PlayerInput playerInput;
    private Rigidbody rb;

    // keeps track of how the player is trying to move.
    private Vector3 movement;

    /**
     * Awake is called IMMEDIATELY as this script is instantiated.
     * I mean seriously. It's the very first function Unity calls.
     */
    private void Awake() {
        /**
         * Creates a new instance of the PlayerInput object.
         * Note that PlayerInput is actually a script! It's being
         * generated inside of the Scripts/Generated folder.
         */
        playerInput = new PlayerInput();
        /**
         * This is basically a callback. It saves the OnMove function
         * inside of the Move.started, performed, and canceled event,
         * so any time the user tries to move the player, it will
         * execute the OnMove function.
         */
        playerInput.Player.Move.started += OnMove;
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;

        /**
         * If the GameObject this script is on (the player) has a Rigidbody
         * (which it always should), grab it.
         */
        rb = GetComponent<Rigidbody>();
    }

    /**
     * Enables and disables the input reader. This is important so that
     * we avoid doing any work taking user input if the player is supposed
     * to be disabled.
     */
    private void OnEnable() { playerInput.Player.Enable(); }
    private void OnDisable() { playerInput.Player.Disable(); }

    /**
     * Runs once for every physics update. This is different from
     * normal Update() which runs once for every rendering update.
     */
    private void FixedUpdate() {
        Vector3 newVelocity = movement;
        /**
         * newVelocity is now sped up by our variable and then adjusted by fixedDeltaTime.
         * The fixedDeltaTime is very important because otherwise the player's movement
         * speed would vary between a high end and a low end PC.
         */
        newVelocity *= moveSpeed * Time.fixedDeltaTime;
        // Don't lose the current upwards velocity, if any.
        newVelocity.y = rb.velocity.y;
        // Update velocity! Done.
        rb.velocity = newVelocity;
        // For the time being, don't allow the player to gain angular velocity.
        rb.angularVelocity = Vector3.zero;
    }

    /**
     * The function that is called every time the user tries to do anything
     * that would be considered a Move input.
     */
    private void OnMove(InputAction.CallbackContext ctx) {
        /*
         * Save the player's movement into the movement vector.
         * ReadValue is just a special function that Unity's InputSystem
         * uses to pull out whatever the result of the input was.
         *
         * Since the input (from WASD, arrow keys, a joystick, etc.) will be a
         * Vector2, we have to turn this into a Vector3 based on X and Z instead.
         */
        Vector2 movement2d = ctx.ReadValue<Vector2>();
        movement = new Vector3(movement2d.x, 0, movement2d.y);
    }
}
