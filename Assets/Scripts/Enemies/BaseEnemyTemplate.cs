using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public abstract class BaseEnemyTemplate : MonoBehaviour
{
    protected float health;
    [SerializeField] protected AudioClip FX; //temporary audioclip location
    [SerializeField] protected float speed;
    [SerializeField] protected float attackDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected HealthComponent healthComponent;

    void Awake()
    {
        
    }

    void Start()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.health = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create default behaviour script (like moving?) and stick it into update
    //

    //define each state of the machine
    //define the transitions between states
    //select the initial state
}
