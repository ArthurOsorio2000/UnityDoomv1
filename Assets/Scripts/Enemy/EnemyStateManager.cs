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
    [SerializeField] protected AudioClip enemyWeaponFX;
    [SerializeField] protected AudioClip enemyCallout1;
    [SerializeField] protected AudioClip enemyCallout2;
    [SerializeField] protected AudioClip enemyCallout3;
    List<AudioClip> calloutList = new List<AudioClip>();

    [Header("Enemy Attributes")]
    [SerializeField] protected float enemyHealth = 200f;
    [SerializeField] protected float calloutRange = 20f;

    [Header("Idle State Parameters")]
    [SerializeField] protected float idleSweepAngle = 90f;
    [SerializeField] protected float idleSweepTime = 5f;
    [SerializeField] protected float idleSweepRange = 100f;

    [Header("Patrol State Parameters")]
    [SerializeField] private float patrolSpeed = 5f;

    [Header("Chase State Parameters")]
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Combat State Parameters")]
    [SerializeField] private float combatAtkDamage = 50f;
    [SerializeField] private float combatRange = 15f;
    [SerializeField] [Tooltip("Must be a value between 0 and 1.")] private float combatSpreadRadius = 0f;

     [Header("Debug Fields")]
    [SerializeField] protected bool canSeePlayer;
    [field: SerializeField] protected bool playerDetected {get; set;}

    //variables for finding GameObject with player mesh
    private GameObject[] playerList;
    private GameObject player;
    private Vector3 directionTowardsPlayer;

    //States:
    private EnemyIdleState idleState;

    void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //assign states to state variables
        idleState = new EnemyIdleState();
        //assign values to external Components
        gameObject.layer = 6;
        gameObject.tag = "Enemy";
        healthComponent.health = enemyHealth;
        audioManager = AudioManager.Instance;
        enemyWeaponFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        enemyCallout1 = (AudioClip) Resources.Load("Sounds/Enemy Sounds/Zombieman Sounds/zombiemancallout1", typeof(AudioClip));
        enemyCallout2 = (AudioClip) Resources.Load("Sounds/Enemy Sounds/Zombieman Sounds/zombiemancallout2", typeof(AudioClip));
        enemyCallout3 = (AudioClip) Resources.Load("Sounds/Enemy Sounds/Zombieman Sounds/zombiemancallout3", typeof(AudioClip));
        calloutList.Add(enemyCallout1);
        calloutList.Add(enemyCallout2);
        calloutList.Add(enemyCallout3);
        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList != null){
           player = playerList[0];
        }
        
        //set default state
        SwitchState(State.Idle);
    }

    void Update()
    {
        //temp command - if loses health, will see player.
        // if(healthComponent.health < enemyHealth)
        // {
        //     playerDetected = true;
        // }
        // Debug.Log(transform.forward);

        //if current health is less than previous health and currently not playing pain sound, play pain sound?
    }

//-----------------------------------------------Idle state----------------------------------------------//

    //look around for a while. If player not detected, switch to patrol state
    private bool currentlySweeping = false;
    private IEnumerator IdleState()
    {
        //if the player hasn't been detected, do a raycast sweep in front for a number of seconds
        while(true){
            if(!currentlySweeping){
                currentlySweeping = true;
                //is there a function that does all this automatically?
                float theta = idleSweepAngle / 2;
                theta = theta * (3.14159f/180);
                float angleRight = Mathf.Sin(theta);
                float angleForward = Mathf.Cos(theta);

                //negative initial angleright for left to right sweep
                Vector3 idleSweepInitialVector = new Vector3(-angleRight, 0, angleForward);
                Vector3 idleSweepFinalVector = new Vector3(angleRight, 0, angleForward);

                Vector3 sweepInitialDirection = transform.rotation * idleSweepInitialVector;
                Vector3 sweepFinalDirection = transform.rotation * idleSweepFinalVector;

                Vector3 shotOrigin = transform.position;
                //divide a wait into the amount of times it'll take the for loop to loop vs the idlesweep time then stick it into the end of the for loop?
                for(float t = 0f; t < 1; t += Time.deltaTime / idleSweepTime){
                    RaycastHit hit;
                    Ray ray = new Ray(shotOrigin, Vector3.Slerp(sweepInitialDirection, sweepFinalDirection, t));

                    if(Physics.Raycast(shotOrigin, Vector3.Slerp(sweepInitialDirection, sweepFinalDirection, t), out hit, idleSweepRange))
                    {
                        if(hit.transform.tag == "Player"){
                            IdleDetectPlayerAction();
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 0.1f, false);
                            canSeePlayer = false;
                        }
                    }else
                    {
                        Debug.DrawLine(shotOrigin, ray.origin + ray.direction * 100, Color.blue, 0.1f, false);
                        canSeePlayer = false;
                    }
                    if(playerDetected)
                    {
                        IdleDetectPlayerAction();
                    }
                    yield return new WaitForSeconds(Time.deltaTime / idleSweepTime);
                }
            currentlySweeping = false;
            }
            yield return null;
        }
    }

    void IdleDetectPlayerAction()
    {
        Debug.Log("Can see player");
        //Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f, false);
        playerDetected = true;
        SwitchState(State.Chase);
    }

//-----------------------------------------------patrol state----------------------------------------------//
    //pathfind randomly.
    //will work on later
    public IEnumerator PatrolState()
    {
        navAgent.speed = patrolSpeed;
        //choose a random spot to walk to? or choose a random spot to walk towards and idle?
        while(true){
            Debug.Log("in patrol state");
            yield return null;  
        }
    }

//-----------------------------------------------Surprise state----------------------------------------------//
//if the player hasn't been detected and playerDetected is toggled, enter this mode, play a surprise animation and wait a random amount of time (reactiontime?)
//before entering chase to give player enough time to react to detection


//-----------------------------------------------Chase state----------------------------------------------//

    private bool canTrack = true;
    private bool hasCalledOut = false;
    private IEnumerator ChaseState()
    {
        navAgent.speed = chaseSpeed;
        while(true){
            LookForPlayer();
            //figure out a better place to put this
            if(!hasCalledOut){
                DoCallout();
                hasCalledOut = true;
            }
            //error prevention to disable navagent on death
            if(canTrack){
                navAgent.SetDestination(player.transform.position);
            }
            //if the player is out of eyesight for a while, return to idle?
            //if player is in eyesight and less than a certain range, switch to combat
            if(Vector3.Distance(transform.position, player.transform.position) <= combatRange && canSeePlayer)
            {
                SwitchState(State.Combat);
            }
            yield return null;  
        }
    }

//-----------------------------------------------Combat state----------------------------------------------//

    //note - there is a coroutine active that doesn't stop, so even though enemy can chase player, the look for player coroutine doesn't start until something ends.
    //when enemy is alerted, all active coroutines should be stopped so that look for player takes action straight away - this is likely the idle dosweep coroutine not ending prematurely.
    //add if player is seen, exit and stop coroutine in dosweep coroutine.
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
            LookForPlayer();
            transform.LookAt(player.transform);
            //ensure entire model doesn't tilt towards player during height difference
            transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, 0, 0), transform.eulerAngles.y, transform.eulerAngles.z);

            //combat testing
            while (!combatShot && canSeePlayer){
                //wait a random amount of time before firing at player with a minimum response time of 0.3 seconds?
                DoAttack();
                audioManager.PlaySoundEffect(enemyWeaponFX, transform, 0.7f, 1);
                combatShot = true;
                //start a coroutine to move slightly in a random direction before engaging in firedelay
                StartCoroutine(FireDelay(Random.Range(1f, 1.5f)));
            }
            if (Vector3.Distance(transform.position, player.transform.position) > combatRange || !canSeePlayer)
            {
                SwitchState(State.Chase);
            }
            //once the player is within a certain range of the player, start combat.
            //once the player is outside the maximum combat range, switch to chasing.
            
            yield return null;  
        }
    }

    void DoAttack()
    {
        Vector3 shotOrigin = transform.position;

        Vector3 gunSpread = Random.insideUnitCircle * combatSpreadRadius;
        gunSpread.z = 1;
        Vector3 shotDirection = transform.rotation * gunSpread;

        //for future debugging = maybe instantialise the transform position so it can be reflected to both the ray debugger and
        //the physics raycast - just make sure those values are the same
        RaycastHit hit;
        Ray ray = new Ray(shotOrigin, shotDirection);

        //check for hit
        if (Physics.Raycast(shotOrigin, shotDirection, out hit))
        {
            //Debug.DrawLine(ray.origin, hit.point, Color.red, 2, false);

            //for shot trails
            // lineRenderer = GetComponent<LineRenderer>();
            // lineRenderer.SetPosition(0, ray.origin);
            // lineRenderer.SetPosition(1, hit.point);

            //print whatever the raycast hit to the debug log
            Debug.Log(hit.transform.name);

            //on hit, deal damage to target
            HealthComponent target = hit.transform.GetComponent<HealthComponent>();
            if (target != null)
            {
                target.TakeDamage(combatAtkDamage);
            }
        //action if miss
        }else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 2, false);
            Debug.Log("miss");
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

    //if I want to clamp look height so that enemies don't look straight down, I can clamp the ray angle in update
    //how can I only call this when I need to? should I only call it in the chase and combat states?
    void LookForPlayer()
    {
        RaycastHit hit;
        directionTowardsPlayer = player.transform.position - transform.position;
        Ray enemyVision = new Ray(transform.position, directionTowardsPlayer);
        
        if(Physics.Raycast(transform.position, directionTowardsPlayer, out hit, combatRange))
        {
            if(hit.transform.tag == "Player"){
                Debug.Log("Can see player");
                Debug.DrawLine(enemyVision.origin, hit.point, Color.red, 2, false);
                canSeePlayer = true;
            }
            else
            {
                Debug.DrawLine(enemyVision.origin, enemyVision.origin + enemyVision.direction * 100, Color.blue, 2, false);
                canSeePlayer = false;
            }
        }else
        {
            Debug.DrawLine(enemyVision.origin, enemyVision.origin + enemyVision.direction * 100, Color.blue, 2, false);
            canSeePlayer = false;
        }
    }

    private LayerMask allies = 64; // <-- 6: enemy layer
    private Collider[] enemiesInEarshot;
    private void DoCallout()
    {
        audioManager.PlaySoundEffect(calloutList[Random.Range(0, 2)], transform, 0.7f, 1);
        //play a callout sound ("There he is!" or something) - if surprisestate is implemented, stick this in there, too.
        //get list of enemies in earshot and toggle them to detect player and chase
        //bug - enemy layer doesn't work
        enemiesInEarshot = Physics.OverlapSphere(transform.position, calloutRange, allies);
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