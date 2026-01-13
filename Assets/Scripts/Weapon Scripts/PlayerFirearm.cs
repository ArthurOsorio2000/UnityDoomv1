using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //all the things that need to be changed by the inheriting weapons
    [SerializeField] protected AudioClip fireFX;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float rateOfFire;
    [SerializeField] protected float bulletsPerShot;
    //note all spread radius numbers need to be between 0 and 1, as the unit circle used to randomize
    //the spread has a maximum radius of 1
    [SerializeField] protected float spreadRadius;
    [SerializeField] protected bool isAutomatic;

    //all the things that can stay the same/get inherited
    protected Camera playerCamera;
    protected InputManager inputManager;
    protected AudioManager audioManager;
    
    //if I want to add shot trails
    //protected LineRenderer lineRenderer;

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
    public virtual void Shoot(AudioClip fireFX, float damage, float range, float rateOfFire, float spreadRadius)
    {
        if (canFire){
        audioManager.PlaySoundEffect(fireFX, transform, 1f);
        Vector3 shotOrigin = playerCamera.transform.position;

        Vector3 gunSpread = Random.insideUnitCircle * spreadRadius;
        gunSpread.z = 1;


        Vector3 shotDirection = (gunSpread - Vector3.zero).normalized;
        shotDirection = playerCamera.transform.rotation * shotDirection;

        //for future debugging = maybe instantialise the transform position so it can be reflected to both the ray debugger and
        //the physics raycast - just make sure those values are the same
        RaycastHit hit;
        Ray ray = new Ray(shotOrigin, shotDirection);

        //check for hit
        if (Physics.Raycast(shotOrigin, shotDirection, out hit, range))
        {
            //for debugging
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2, false);

            // lineRenderer =  GetComponent<LineRenderer>();
            // lineRenderer.SetPosition(0, ray.origin);
            // lineRenderer.SetPosition(1, hit.point);

            //print whatever the raycast hit to the debug log
            Debug.Log(hit.transform.name);

            HealthComponent target = hit.transform.GetComponent<HealthComponent>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 2, false);
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
