using System.Collections;
using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //all the things that need to be changed by the inheriting weapons
    protected abstract AudioClip fireFX {get;}
    protected abstract float damage {get;}
    protected abstract float range {get;}
    protected abstract float rateOfFire {get;}
    protected abstract bool isAutomatic {get;}

    //all the things that can stay the same/get inherited
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected InputManager inputManager;

    protected abstract void UpdateWeapon();

    void Start()
    {
        //assign common components
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeapon();
    }

    //default fire method
    //create a coroutine to manage firerate
    //figure out how to to use the isAutomatic bool to specify how the gun controls.
    //should there be a single shot and an automatic shot method? or should the shoot method be changed per gun?

    //initialize variable for default firerate control function in shoot method
    protected bool canFire = true;
    public virtual void Shoot(AudioClip fireFX, float damage, float range, float rateOfFire)
    {
        if (canFire){
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
        canFire = false;
        StartCoroutine(FireDelay(rateOfFire));
        }
    }

    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }
}
