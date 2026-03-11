using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerFirearm : MonoBehaviour
{
    //get the spread of the crosshair and import it across to the reticle so that the spread can affect
    //the radius of the crosshair and dynamically changes based on player movement, firing or playerdamage?
    //all the things that need to be changed by the inheriting weapons
    [Header("SFX")]
    [SerializeField] protected AudioClip fireFX;

    [Header("Weapon Attributes")]
    [SerializeField] public bool inInventory = false;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float spreadRadius;
    [SerializeField] protected float rateOfFire;
    [SerializeField] protected float audibleRange;
    [SerializeField] protected int bulletsPerShot;
    //note all spread radius numbers need to be between 0 and 1, as the unit circle used to randomize
    //the spread has a maximum radius of 1

    //all the things that can stay the same/get inherited
    protected Camera playerCamera;
    protected InputManager inputManager;
    protected AudioManager audioManager;
    protected LayerMask enemyLayer = 64;
    protected Collider[] enemiesInEarshot;

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

    void SetActive()
    {
        
    }

    //if this isn't stated - if weapon is disabled while canFire is disabled, weapon will not fire
    void OnEnable()
    {
        canFire = true;
    }

    void AddToInventory()
    {
        inInventory = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeapon();
    }
    
    //debug: to visualise audio range
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, audibleRange);
    }

    protected bool canFire = true;
    public virtual void Shoot(AudioClip fireFX, float damage, float range, float spreadRadius, float rateOfFire, float audibleRange, int bulletsPerShot = 1)
    {
        if (canFire){
            audioManager.PlaySoundEffect(fireFX, transform, 0.2f, 0);
            //get list of enemies in earshot and toggle them to detect player and chase
            //bug - enemy layer doesn't work
            enemiesInEarshot = Physics.OverlapSphere(transform.position, audibleRange, enemyLayer);
            foreach (Collider enemyInEarshot in enemiesInEarshot)
            {
                print("enemy heard: " + enemyInEarshot.gameObject.name);
                EnemyStateManager enemy = enemyInEarshot.GetComponent<EnemyStateManager>();

                if (enemy != null)
                {
                    enemy.HearPlayer(true);
                }
            }

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
            //action if hit
            //Debug.DrawLine(ray.origin, hit.point, Color.red, 2, false);

            //for shot trails
            // lineRenderer = GetComponent<LineRenderer>();
            // lineRenderer.SetPosition(0, ray.origin);
            // lineRenderer.SetPosition(1, hit.point);

            //print whatever the raycast hit to the debug log
            Debug.Log(hit.transform.name);

            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if(damagable != null){
                damagable.Damage(damage);
            }
        //action if miss
        }else
        {
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 2, false);
        }
    }

    //can be toolboxed - also appears in EnemyStateManager
    IEnumerator FireDelay(float rateOfFire)
    {
        yield return new WaitForSeconds(rateOfFire);
        canFire = true;
    }
}
