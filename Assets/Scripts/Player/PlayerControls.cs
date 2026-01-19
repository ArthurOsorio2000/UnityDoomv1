using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    //should playercontrols be a singleton?
    private static PlayerControls _instance;
    public static PlayerControls Instance
    {
        get
        {
            return _instance;
        }
    }
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float gravityValue = -50f;

    private CharacterController controller;
    private InputManager inputManager;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Transform playerCameraTransform;


    private void Start()
    {
        PlayerControlsSingletonCheck();
        inputManager = InputManager.Instance;
        controller = GetComponent<CharacterController>();
        playerCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        //gameObject.tag = "Player";
    }

    void PlayerControlsSingletonCheck()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }


    //how do you handle player movement going up and down inclines

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }
        
        // Read input
        Vector2 moveInput = inputManager.GetPlayerMovement();
        //look input doesn't work because it's a delta pass through value - it only applies during movement.
        //I need to find the value of something that actually turns during camera movement and copy that instead.

        //note to self - this is the magnitude of a movement vector per frame, not the direction
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        if (move != Vector3.zero){
            transform.forward = move;
        }
        //rotate player to change forward vector based on player camera
        transform.rotation = Quaternion.Euler(0, playerCameraTransform.eulerAngles.y, 0);

        //does this normalize a combination of vectors, like a diagonal input, to prevent non-uniform speeds in certain directions?
        //for the sake of doom, I'll leave the movement vector magnitude unclamped to mimic movement tech
        //move = Vector3.ClampMagnitude(move, 1f); 

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        //Vector3.up * playerVelocity.y adds gravity
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(transform.rotation * finalMove * Time.deltaTime);
    }
}