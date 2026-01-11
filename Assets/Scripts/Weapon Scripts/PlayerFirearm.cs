using System.Collections;
using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //all the things that need to be changed by the inheriting weapons
    [SerializeField] protected AudioClip fireFX;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float rateOfFire;
    [SerializeField] protected bool isAutomatic;

    //all the things that can stay the same/get inherited
    protected Camera playerCamera;
    protected InputManager inputManager;
    protected AudioManager audioManager;

    protected abstract void UpdateWeapon();

    void Start()
    {
        //assign common components
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        inputManager = InputManager.Instance;
        audioManager = AudioManager.Instance;
    }

    void OnEnable()
    {
        canFire = true;
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
        audioManager.PlaySoundEffect(fireFX, transform, 1f);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            //print whatever the raycast hit to the debug log
            Debug.Log(hit.transform.name);

            //
            HealthComponent target = hit.transform.GetComponent<HealthComponent>();
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
