using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions playerControls;
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        playerControls = new InputSystem_Actions();
        SingletonCheck();
    }

    //ensure there is one and only one instance of this class
    void SingletonCheck()
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerControls.Enable();
        
    }
    

    void OnDisable ()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 Look()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }


    //note to self - difference between triggered and IsPressed:
    public bool PlayerShooting()
    {
        return playerControls.Player.Shoot.IsPressed();
    }
}
