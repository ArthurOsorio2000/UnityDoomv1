using System.Collections;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    protected Camera playerCamera;
    [SerializeField] private int currentWeapon;

    //note: old weapon inventory
    [SerializeField] private GameObject[] weapons;

    //private prefabs of weapons to go into playerWeaponInventory
    [SerializeField] private GameObject playerHandgun;
    [SerializeField] private GameObject playerAssaultRifle;
    [SerializeField] private GameObject playerShotgun;
    
    /**note: new - this holds all the weapons. How do you populate this hashtable with PlayerFirearm objects
    then attach the correct correlating initial components (Handgun, AssaultRifle, Shotgun etc.) to the
    game object?**/
    [SerializeField] private Hashtable playerWeaponInventory = new Hashtable();
    [SerializeField] private InputManager inputManager;

    void Awake()
    {
        //create gameobjects and add correlating components - is there a way to reduce playerfirearm assignments?
        //for all objects in hashtable, addcomponent playerfirearm?
        playerHandgun.AddComponent<PlayerFirearm>();
        playerAssaultRifle.AddComponent<PlayerFirearm>();
        playerShotgun.AddComponent<PlayerFirearm>();

        playerHandgun.AddComponent<Handgun>();
        playerAssaultRifle.AddComponent<AssaultRifle>();
        playerShotgun.AddComponent<Shotgun>();


        //instantiate player weapons
        Instantiate(playerHandgun, new Vector3(0, 0, 0), Quaternion.Identity);
        Instantiate(playerAssaultRifle, new Vector3(0, 0, 0), Quaternion.Identity);
        Instantiate(playerShotgun, new Vector3(0, 0, 0), Quaternion.Identity);

        //add player weapons to weapon inventory hashtable
        playerWeaponInventory.Add("PlayerHandgun", playerHandgun);
        playerWeaponInventory.Add("PlayerAssaultRifle", playerAssaultRifle);
        playerWeaponInventory.Add("PlayerShotgun", playerShotgun);
    }

    void Start()
    {
        //assign common components
        inputManager = InputManager.Instance;
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //<old> go through each object in array and set SetActive to false.
        // - this array is filled prior to Start
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        EquipDefaultWeapon();
        weapons[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
        //constantly track player inputs
        EquipHangun();
        EquipAssaultRifle();
        EquipShotgun();
    }

    void EquipDefaultWeapon()
    {
        //select handgun as default weapon
        SwapWeapon(0);
    }

    void EquipHangun()
    {
        if (inputManager.SelectHandgun())
        {
            SwapWeapon(0);
        }
    }
    void EquipAssaultRifle()
    {
        if (inputManager.SelectAssaultRifle())
        {
            SwapWeapon(1);
        }
    }

    void EquipShotgun()
    {
        if (inputManager.SelectShotgun())
        {
            SwapWeapon(2);
        }
    }

    void SwapWeapon(int selectedWeapon)
    {
        if(selectedWeapon.inInventory){
            if(selectedWeapon != currentWeapon) //here, when implemented, also check if the weapon has been collected, yet
                {
                    weapons[currentWeapon].SetActive(false);
                    weapons[selectedWeapon].SetActive(true);
                    currentWeapon = selectedWeapon;
                }
        }
        else
        {
            Debug.Log("Weapon not currently in inventory");
        }
    }
}