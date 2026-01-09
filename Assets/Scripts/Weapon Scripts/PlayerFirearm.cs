using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //all the things that need to be changed by the inheriting weapons
    protected abstract AudioClip fireFX {get;}
    protected abstract float damage {get;}
    protected abstract float range {get;}

    //all the things that can stay the same/get inherited
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected InputManager inputManager;

    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range);
        }
    }

    //default fire method
    public virtual void Shoot(AudioClip fireFX, float damage, float range)
    {
        AudioManager.Instance.PlaySoundEffect(fireFX, transform, 1f);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            //print whatever the raycast hit to the debug log
            Debug.Log(hit.transform.name);

            //
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
