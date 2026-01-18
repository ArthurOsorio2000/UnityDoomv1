using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


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
    //Dictionary<int, string> states = new Dictionary<int, string>();
    
    [System.Serializable] public enum State {Idle, Patrol, Chase, Combat};
    Coroutine _activeState;

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
    [SerializeField] private float enemyRange = 15f;

    private bool canTrack = true;

    void Awake()
    {
        // states.Add(1, "Idle");
        // states.Add(2, "Patrol");
        // states.Add(3, "Combat");
        healthComponent = GetComponent<HealthComponent>();
        agent = GetComponent<NavMeshAgent>();
    }

    //like components for each enemy type should be instantiated here,
    //but values should be assigned in a state so different enemies can set their own health
    void Start()
    {
        //assign values to external Components
        healthComponent.health = enemyHealth;
        audioManager = AudioManager.Instance;
        enemyWeaponFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList != null){
           player = playerList[0];
        }

        SwitchState(State.Idle);

        float dist = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log("Distance to player: " + dist);
    }

    void Update()
    {

    }

    public void SwitchState(State destinationState)
    {
        IEnumerator state = null;
        switch(destinationState) {
            case State.Idle : state = IdleState(); break;
            case State.Patrol : state = PatrolState(); break;
            case State.Chase : state = ChaseState(); break;
            case State.Combat : state = CombatState(); break;
        }
       
       if(_activeState != null)
        {
            StopCoroutine(_activeState);
        }

        _activeState = StartCoroutine(state);
    }

    private IEnumerator IdleState()
    {
        while(true){
            Debug.Log("in Idle state");
            SwitchState(State.Chase);
            yield return null;
        }
    }

    public IEnumerator PatrolState()
    {
        while(true){
            Debug.Log("in patrol state");
            yield return null;  
        }
    }

    private IEnumerator ChaseState()
    {
        while(true){
            //if the agent is in the middle of doing this while being destroyed, it will throw an error?
            if(canTrack){
                agent.SetDestination(player.transform.position);
            }
            //if player is in eyesight and less than a certain range, switch to combat
            if(Vector3.Distance(transform.position, player.transform.position) <= enemyRange)
            {
            //when within range, fire raycast in the direction of the player.
            //if hit target is not player, keep chasing.
            //if hit target is the player, this means the player is in eyesight - ie not visually blocked by anything.
            //switch to combat.
                RaycastHit hit;
                Ray playerVision = new Ray(transform.position, transform.forward);

                if(Physics.Raycast(transform.position, transform.forward, out hit, enemyRange))
                {
                    if(hit.transform.tag == "Player"){
                        Debug.Log("Can see player");
                        //Debug.DrawLine(playerVision.origin, hit.point, Color.red, 2, false);
                        SwitchState(State.Combat);
                    }
                }
            }
            
            
            yield return null;  
        }
    }

    private IEnumerator CombatState()
    {
        //fire at the player.
        if(canTrack){
            agent.SetDestination(transform.position);
        }
        Debug.Log("I am fighting now");
        while(true){
            transform.LookAt(player.transform);
            if(Vector3.Distance(transform.position, player.transform.position) > enemyRange)
            {
                SwitchState(State.Chase);
            }
            //once the player is within a certain range of the player, start combat.
            //once the player is outside the maximum combat range, switch to chasing.
            
            yield return null;  
        }
    }

    // while (!combatShot){
                //     audioManager.PlaySoundEffect(enemyWeaponFX, transform, 1f);
                //     combatShot = true;
                //     StartCoroutine(FireDelay(1f));
                // }
    bool combatShot = true;
    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        combatShot = false;
    }

    void OnDestroy()
    {
        canTrack = false;
        Destroy(this);
    }

}