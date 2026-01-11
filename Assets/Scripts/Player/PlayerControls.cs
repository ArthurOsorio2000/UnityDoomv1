using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController controller;
    private InputManager inputManager;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Transform playerCameraTransform;


    private void Start()
    {
        inputManager = InputManager.Instance;
        controller = GetComponent<CharacterController>();
        playerCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }


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
        //controller.transform.rotation = quaternion.Euler(0, cameraRotation.transform.rotation.y, 0);

        //note to self - this is the magnitude of a movement vector per frame, not the direction
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        if (move != Vector3.zero){
            transform.forward = move;
        }
        transform.rotation = Quaternion.Euler(0, playerCameraTransform.eulerAngles.y, 0);

        //does this normalize a combination of vectors, like a diagonal input, to prevent non-uniform speeds in certain directions?
        //for the sake of doom, I'll leave the movement vector magnitude unclamped to mimic movement tech
        //move = Vector3.ClampMagnitude(move, 1f); 

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(transform.rotation * finalMove * Time.deltaTime);
    }
}