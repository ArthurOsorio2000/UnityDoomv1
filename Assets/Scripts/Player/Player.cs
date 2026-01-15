using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(PlayerControls))]
public class Player : MonoBehaviour
{
    public HealthComponent playerHealthComponent;
    public PlayerControls playerControls;

    //use class to instantiate stuff that player needs? how necessary is this?
    //at least summon player health using this after prototyping?
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (playerHealthComponent == null)
        {
            playerHealthComponent = GetComponent<HealthComponent>();
        }

        if (playerControls == null)
        {
            playerControls = PlayerControls.Instance;
        }
    }
    
    void Start()
    {
        //instantiate playercontrols compoenent onto the attached object <-- how would I do this? playercontrols is on
        //a playercontroller object, while the player is a holder for the entire model. Would this be on the same game
        //object as the player controller?
        playerHealthComponent.health = 200;
        //also instantiate and edit the initial health of a health component
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
