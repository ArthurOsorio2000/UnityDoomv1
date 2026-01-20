using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateManager : MonoBehaviour
{
    //components
    private HealthComponent healthComponent;
    private AudioManager audioManager;
    private NavMeshAgent navAgent;
    
    [System.Serializable] public enum State {Idle, Patrol, Chase, Combat};

     [Header("SFX")]
    [SerializeField] private AudioClip enemyWeaponFX;

     [Header("Enemy Attributes")]
    [SerializeField] private float enemySpeed = 5f;
    [SerializeField] private float enemyHealth = 200f;
    [SerializeField] private float enemyAtkDamage = 50f;
    [SerializeField] private float enemyRange = 15f;
    [SerializeField] private float audibleRange = 20f;


     [Header("Debug Fields")]
    [SerializeField] private bool canSeePlayer;
    [field: SerializeField] private bool playerDetected {get; set;}

    //variables for finding GameObject with player mesh
    private GameObject[] playerList;
    private GameObject player;

    void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //assign values to external Components
        gameObject.layer = 6;
        healthComponent.health = enemyHealth;
        audioManager = AudioManager.Instance;
        enemyWeaponFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList != null){
           player = playerList[0];
        }
        
        //set default state
        SwitchState(State.Idle);
    }

    void Update()
    {
        LookForPlayer();
        //temp command - if loses health, will see player.
        if(healthComponent.health < enemyHealth)
        {
            playerDetected = true;
        }
    }

    //if I want to clamp look height so that enemies don't look straight down, I can clamp the ray angle in update
    void LookForPlayer()
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

//-----------------------------------------------Idle state----------------------------------------------//

    //look around for a while. If player not detected, switch to patrol state
    private IEnumerator IdleState()
    {
        //if the player hasn't been detected, do a raycast sweep in front for a number of seconds
        while(true){
            if (!playerDetected)
            {
                Debug.Log("in Idle state");
            }
            else
            {
                SwitchState(State.Chase);
            }
            yield return null;
        }
    }

    void DoSweep()
    {
        RaycastHit hit;
        //for loop? should this be in an IEnumerator? should this behaviour be in the Idle state instead?
        Vector3 directionTowardsPlayer = player.transform.position - transform.position;
        //Ray enemyVision = new Ray(transform.position, directionTowardsPlayer);
        
        
        if(Physics.Raycast(transform.position, directionTowardsPlayer, out hit, enemyRange))
        {
            if(hit.transform.tag == "Player"){
                //Debug.Log("Can see player");
                //Debug.DrawLine(enemyVision.origin, hit.point, Color.red, 2, false);
                canSeePlayer = true;
            }
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

//---------------------------------------- Code for look sweep ---------------------------------------------
//     //should looking be a separate state? like, sweep state where the enemy sweeps an area to look around?
//     //then switches to partrol state which raycasts around to check for a direction, then walks in that direction?
//     //so that during combat, when enemies have lost sight of the player for a number of seconds, they can enter sweep state?
//     IEnumerator IdleLook(EnemyStateManager enemy, float lookTime, float lookLength = 3f, float lookRadius = 179f)
//     {
//         lookRadius = lookRadius / 2; // <-- randomise the initial orientation with a random seed generated by each instance of the enemy class so the initial direction is unique for each instance
//         //variable for looking to one side
//         float firstLook = lookLength / 6;
//         //covering distance from the first side to the second side
//         float secondLook = firstLook * 2;        

//         //if enemy is shot at - stop these coroutines, then stop the partrol look coroutine in Enterstate and shift to combatstate
//         //can you nest these coroutines in itself recursively?
//         lookCoroutine = SweepArea(enemy, firstLook, lookRadius);
//         enemy.StartCoroutine(lookCoroutine);
//         yield return new WaitForSeconds(lookTime);
//         lookCoroutine = SweepArea(enemy, secondLook, -lookRadius * 2);
//         enemy.StartCoroutine(lookCoroutine);
//         yield return new WaitForSeconds(lookTime);
//         lookCoroutine = SweepArea(enemy, firstLook, lookRadius);
//         enemy.StartCoroutine(lookCoroutine);

//     }

//     IEnumerator SweepArea(EnemyStateManager enemy, float turnLength = 3f, float lookRadius = 179f, float lookLength = 1)
//     {
//         //first look

//         Vector3 byAngles = new Vector3(0f, lookRadius, 0f);
//         Quaternion fromAngle = enemy.transform.rotation;
//         Quaternion toAngle = Quaternion.Euler(enemy.transform.eulerAngles + byAngles);
//         for(var t = 0f; t < 1; t += Time.deltaTime / turnLength)
//         {
//             enemy.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
//             yield return null;
//         }
//     }

//-----------------------------------------------patrol state----------------------------------------------//
    //pathfind randomly.
    public IEnumerator PatrolState()
    {
        //choose a random spot to walk to? or choose a random spot to walk towards and idle?
        while(true){
            Debug.Log("in patrol state");
            yield return null;  
        }
    }

//-----------------------------------------------Chase state----------------------------------------------//

    private bool canTrack = true;
    private bool hasCalledOut = false;
    private IEnumerator ChaseState()
    {
        
        while(true){
            //figure out a better place to put this
            if(hasCalledOut == false){
                DoCallout();
            }
            //error prevention to disable navagent on death
            if(canTrack){
                navAgent.SetDestination(player.transform.position);
            }
            //if the player is out of eyesight for a while, return to idle?
            //if player is in eyesight and less than a certain range, switch to combat
            if(Vector3.Distance(transform.position, player.transform.position) <= enemyRange && canSeePlayer)
            {
                SwitchState(State.Combat);
            }
            yield return null;  
        }
    }

//-----------------------------------------------Combat state----------------------------------------------//

    private bool combatShot = false;
    private IEnumerator CombatState()
    {
        //going into this state it is assumed canSeePlayer is always true
        //implement canSeePlayer check
        //fire at the player.
        if(canTrack){
            navAgent.SetDestination(transform.position);
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
            if (Vector3.Distance(transform.position, player.transform.position) > enemyRange || !canSeePlayer)
            {
                hasCalledOut = false;
                SwitchState(State.Chase);
            }
            //once the player is within a certain range of the player, start combat.
            //once the player is outside the maximum combat range, switch to chasing.
            
            yield return null;  
        }
    }

//-----------------------------------------------Toolbox methods----------------------------------------------//

    Coroutine _activeState;
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

    private LayerMask enemyLayer = 64;
    private Collider[] enemiesInEarshot;
    private void DoCallout()
    {
        //get list of enemies in earshot and toggle them to detect player and chase
        //bug - enemy layer doesn't work
        enemiesInEarshot = Physics.OverlapSphere(transform.position, audibleRange, enemyLayer);
        foreach (Collider enemyInEarshot in enemiesInEarshot)
        {
            print("alerted friend: " + enemyInEarshot.gameObject.name);
            EnemyStateManager enemy = enemyInEarshot.GetComponent<EnemyStateManager>();
            
            if (enemy != null && enemy.playerDetected == false)
            {
                enemy.HearPlayer(true);
            }
        }
        hasCalledOut = true;
    }

    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        combatShot = false;
    }

    private void OnDestroy()
    {
        canTrack = false;
        Destroy(this);
    }

    public void HearPlayer (bool heardPlayer)
    {
        playerDetected = heardPlayer;
    }
}