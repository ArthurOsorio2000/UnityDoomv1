using UnityEditor;
using UnityEngine;

//should this also be an abstract class to be inherited from?
//every class that inherits this will require a takedamage script?
public class HealthComponent : MonoBehaviour
{
    //if this class isn't on an object with a mesh, then it won't be affected. This means this class needs to be on something with a mesh?
    //but how do I delete the class that parents it? does this mean that the entity that I shoot will dissappear but not it's parent unless
    //I put the mesh on the topmost parent class?
    
    //how do you dynamically change the health value based on the script this is placed on?
    //for enemies with differing health pools, shall I force it to require a HealthComponent and it's relevant sized hitbox, then override
    //the health variable with the intended health, to be further overriden as a SerializeField in-editor?
    [field: SerializeField] public float health {get; set;} = 50f;

    public void TakeDamage (float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();
            } 
        }

    void Die()
        {
            //gameObject.SetActive(false) is a good way to reuse stuff if you're going to use it again
            //(respawning enemies or reusing bullets)
            //Destroy(gameObeject) is a good way to save memory. depending on what you need, do either or
            //it's like sending to graveyard vs exile lol

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }