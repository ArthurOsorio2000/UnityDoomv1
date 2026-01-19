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
    [SerializeField] private bool canSeePlayer;

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
        CanSeePlayer();
    }

    void CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 directionTowardsPlayer = player.transform.position - transform.position;
        //Ray enemyVision = new Ray(transform.position, directionTowardsPlayer);
        
        
        if(Physics.Raycast(transform.position, directionTowardsPlayer, out hit, enemyRange))
        {
            if(hit.transform.tag == "Player"){
                //Debug.Log("Can see player");
                //Debug.DrawLine(enemyVision.origin, hit.point, Color.red, 2, false);
                canSeePlayer = true;
            }
            //the reason no drawlines were appearing during the bug were because they were hitting this point and not triggering anything
            else
            {
                //Debug.DrawLine(enemyVision.origin, enemyVision.origin + enemyVision.direction * 100, Color.blue, 2, false);
                canSeePlayer = false;
            }
        }else
        {
            //Debug.DrawLine(enemyVision.origin, enemyVision.origin + enemyVision.direction * 100, Color.blue, 2, false);
            canSeePlayer = false;
        }
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
            if(Vector3.Distance(transform.position, player.transform.position) <= enemyRange && canSeePlayer)
            {
                SwitchState(State.Combat);
            }
            yield return null;  
        }
    }

    bool combatShot = false;
    private IEnumerator CombatState()
    {
        //going into this state it is assumed canSeePlayer is always true
        //implement canSeePlayer check
        //fire at the player.
        if(canTrack){
            agent.SetDestination(transform.position);
        }
        Debug.Log("I am fighting now");

        while(true){
            transform.LookAt(player.transform);
            //ensure entire model doesn't tilt towards player during height difference
            transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, 0, 0), transform.eulerAngles.y, transform.eulerAngles.z);
            
            //combat testing
            while (!combatShot && canSeePlayer){
                //wait a random amount of time before firing at player with a minimum response time of 0.3 seconds?
                audioManager.PlaySoundEffect(enemyWeaponFX, transform, 0.7f, 1);
                combatShot = true;
                //start a coroutine to move slightly in a random direction before engaging in firedelay
                StartCoroutine(FireDelay(Random.Range(1f, 1.5f)));
            }
            if (Vector3.Distance(transform.position, player.transform.position) > enemyRange)
            {
                SwitchState(State.Chase);
            }
            //once the player is within a certain range of the player, start combat.
            //once the player is outside the maximum combat range, switch to chasing.
            
            yield return null;  
        }
    }

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