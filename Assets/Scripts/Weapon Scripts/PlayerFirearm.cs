using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //all the things that need to be changed by the inheriting weapons
    //note, changing these in editor doesn't change anything about the weapon - test why
    [SerializeField] protected AudioClip fireFX;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float rateOfFire;
    [SerializeField] protected int bulletsPerShot;
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

    //initialize variable for default firerate control function in shoot method
    
    //if player isn't moving, I can either remove spread or make it 0?
    protected bool canFire = true;
    //learning optional parameter - adding a value makes it default, and omitting it lets you only have to add it for certain uses
    public virtual void Shoot(AudioClip fireFX, float damage, float range, float spreadRadius, float rateOfFire, int bulletsPerShot = 1)
    {
        if (canFire){
        audioManager.PlaySoundEffect(fireFX, transform, 1f);

        for(int i = 0; i < bulletsPerShot; i++){
            FireBullet(damage, range, spreadRadius);
        }

        //weapon firerate
        canFire = false;
        StartCoroutine(FireDelay(rateOfFire));
        }
    }

    protected void FireBullet(float damage, float range, float spreadRadius)
    {
        Vector3 shotOrigin = playerCamera.transform.position;
        //designate spread
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
                target.TakeDamage(damage);
            }
        //action if miss
        }else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 2, false);
        }
    }

    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }
}
