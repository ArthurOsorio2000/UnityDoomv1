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
        DontDestroyOnLoad(this);
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

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 Look()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }


    //note to self - difference between triggered and IsPressed:
    //how do I make this efficient? how do I make sure hold shot isn't active when single shot is active?
    //note to self for optimization
    public bool PlayerHoldShot()
    {
        return playerControls.Player.Shoot.IsPressed();
    }

    public bool PlayerSingleShot()
    {
        return playerControls.Player.Shoot.triggered;
    }

    public bool SelectHandgun()
    {
        return playerControls.Player.EquipHandgun.triggered;
    }
    public bool SelectAssaultRifle()
    {
        return playerControls.Player.EquipAssaultRifle.triggered;
    }
}