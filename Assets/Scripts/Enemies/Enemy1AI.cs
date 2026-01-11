using UnityEngine;

public class Enemy1AI : MonoBehaviour
{
    //should I have this inherit off a target parent class that handles shots?
    //that would imply both enemies and damagable objects like explosive barrels
    //and boxes would be sibling classes. Would this have any problems?
    
    //both classes require code that makes them targetable. Would this mean that the Player class
    //would then need to inherit this target, or hittableObject class as well?


    //first, make the enemy move randomly (pathfind?).
    //then, make the enemy take damage
    //then, give the enemy states?
    //or, first, look into state control?
    //do I have to look into colliders as well? for enemy wakeup and state change?
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
