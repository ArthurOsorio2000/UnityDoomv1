using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(HealthComponent))]
public class EnemyStateManager : MonoBehaviour
{


    //add thing on top
    //literally just cut everything down to one class
    [SerializeField] private int currentState;
    
    //create a list of states - maybe a dictionary with numbers being a key and a string of the state being a value?

    //in update - constantly keep polling a statemachine switch to keep track of the current state
    //eg if in combat state run the combat state method in update
    //if in patrol state run the patrol state method in update
    Dictionary<int, string> states = new Dictionary<int, string>();

    // public EnemySpawnState SpawnState = new EnemySpawnState();
    // public EnemyIdleState IdleState = new EnemyIdleState();
    // public EnemyPatrolState PatrolState = new EnemyPatrolState();
    // public EnemyCombatState CombatState = new EnemyCombatState();

    //Enemy Attributes - these should all be set in the spawnstate?
    private HealthComponent healthComponent;
    private AudioManager audioManager;
    [SerializeField] private AudioClip enemyWeaponFX;
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float enemyHealth = 200f;
    [SerializeField] private float enemyAtkDamage = 50f;

    void Awake()
    {
        states.Add(1, "Idle");
        states.Add(2, "Patrol");
        states.Add(3, "Combat");
    }

    //like components for each enemy type should be instantiated here,
    //but values should be assigned in a state so different enemies can set their own health
    void Start()
    {
        //assign values to external Components
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.health = enemyHealth;
        audioManager = AudioManager.Instance;

        currentState = 1;

        enemyWeaponFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));

    }

    void Update()
    {
        //keep in mind this is polling every frame. once you start the game, this is playing a method every single time
        //maybe start a coroutine with a flag instead?
        switch (states[currentState])
        {
            case "Idle":
                while(states[currentState] == "Idle"){
                    IdleState();
                }
                break;
            case "Patrol":
                while(states[currentState] == "Patrol"){
                    PatrolState();
                }
                break;
            case "Combat":
                while(states[currentState] == "Combat"){
                    CombatState();
                }
                break;
            default:
            break;
        }
    }

    public void SwitchState()
    {
        
    }

    public void PatrolState()
    {
        Debug.Log("in patrol state");
    }

    private void CombatState()
    {
        bool shot = false;
        while (!shot){
            audioManager.PlaySoundEffect(enemyWeaponFX, transform, 1f);
            shot = true;
        }
    }

    private void IdleState()
    {
        Debug.Log("in Idle state");
    }

    //should the look function be here, so that enemies in idle and combat state can look around as well?
}