using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControls : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private float _xRotation;
    private float _yRotation;
    public float CameraYRotation
    {
        get
        {
            return _yRotation;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = InputManager.Instance;
        //turn this on upon deployment to disable the cursor - but leave off for troubleshooting
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = inputManager.Look().x *Time.deltaTime * sensX;
        float mouseY = inputManager.Look().y *Time.deltaTime * sensX;

        _yRotation += mouseX;
        _xRotation -= mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        PlayerInteract();
    }

    void PlayerInteract()
    {
        if (inputManager.PlayerInteract())
        {
            //fire a raycast - if the hit is a gameobject interactable, call it's interact function
            Vector3 shotOrigin = transform.position;
            Vector3 shotDirection = transform.forward;

            RaycastHit  hitInfo;
            if (Physics.Raycast(shotOrigin, shotDirection, out hitInfo, 2f)){
                IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
                if(interactable != null){
                    interactable.Interact();
                }
            }
        }
    }
}
