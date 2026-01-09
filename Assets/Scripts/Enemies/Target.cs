using UnityEngine;

//should this also be an abstract class to be inherited from?
//every class that inherits this will require a takedamage script?
public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}