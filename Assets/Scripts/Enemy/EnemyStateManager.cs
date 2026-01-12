using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class EnemyStateManager : MonoBehaviour
{
    //states
    //note to self - problem: these states means that for each prefab that spawns with this state manager,
    //each of these spawn states will also be created alongside each state managed gameobject at runtime.
    //singleton-ing doesn't work as the classes aren't monobehaviour and cannot be destroyed on call.
    //there might be a way to only call one existing script for each one, but I'm not sure yet.
    EnemyBaseState currentState;
    public EnemySpawnState SpawnState = new EnemySpawnState();
    public EnemyIdleState IdleState = new EnemyIdleState();
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyCombatState CombatState = new EnemyCombatState();

    //Enemy Attributes - these should all be set in the spawnstate?
    public HealthComponent healthComponent {get; set;}
    public AudioClip FX; //temporary audioclip location
    public float speed;
    public float attackDamage;

    void Awake()
    {
        //assign value to imported Components
        healthComponent = GetComponent<HealthComponent>();
    }

    void Start()
    {
        currentState = SpawnState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    //if health = 0, die?
    //how do I update all states that this enemy is getting shot?
    void Update()
    {
        currentState.UpdateState(this);
        if(healthComponent.health == 0)
        {
            Debug.Log("deathstate reached");
            //currentState = DeadState;
        }
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}