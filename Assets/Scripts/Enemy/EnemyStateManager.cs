using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateManager : MonoBehaviour
{

    private NavMeshAgent agent;
    private GameObject[] playerList;
    private GameObject player;

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
        agent = GetComponent<NavMeshAgent>();
        healthComponent.health = enemyHealth;
        audioManager = AudioManager.Instance;

        currentState = 1;

        enemyWeaponFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));

        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList != null){
           player = playerList[0];
        }
    }

    void Update()
    {
        //keep in mind this is polling every frame. once you start the game, this is playing a method every single time
        //maybe start a coroutine with a flag instead?
        switch (states[currentState])
        {
            case "Idle":
                IdleState();
                break;
            case "Patrol":
                PatrolState();
                break;
            case "Combat":
                CombatState();
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

    bool combatShot = true;
    private void CombatState()
    {
        agent.SetDestination(player.transform.position);

        // while (!combatShot){
        //     audioManager.PlaySoundEffect(enemyWeaponFX, transform, 1f);
        //     combatShot = true;
        //     StartCoroutine(FireDelay(1f));
        // }
    }

    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        combatShot = false;
    }

    private void IdleState()
    {
        Debug.Log("in Idle state");
    }

    //should the look function be here, so that enemies in idle and combat state can look around as well?
}