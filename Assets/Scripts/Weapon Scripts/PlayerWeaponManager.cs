using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    protected Camera playerCamera;
    [SerializeField] private string currentWeaponKey;

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
        
    }

    void Start()
    {
        //assign common components
        inputManager = InputManager.Instance;
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //instantiate player weapons
        //make method to instantiate all weapons - look for child object - if it does not exist, make one
        // playerHandgun = Instantiate(Resources.Load("Weapons/Handgun"), new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameObject>();
        // playerAssaultRifle = Instantiate(Resources.Load("Weapons/Assault Rifle"), new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameObject>();
        // playerShotgun = Instantiate(Resources.Load("Weapons/Shotgun"), new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameObject>();

        //create gameobjects and add correlating components - is there a way to reduce playerfirearm assignments?
        //for all objects in hashtable, addcomponent playerfirearm?
        // playerHandgun.AddComponent<Handgun>();
        // playerAssaultRifle.AddComponent<AssaultRifle>();
        // playerShotgun.AddComponent<Shotgun>();

        //add player weapons to weapon inventory hashtable
        playerWeaponInventory.Add("PlayerHandgun", playerHandgun);
        playerWeaponInventory.Add("PlayerAssaultRifle", playerAssaultRifle);
        playerWeaponInventory.Add("PlayerShotgun", playerShotgun);

        //<old> go through each object in array and set SetActive to false.
        // - this array is filled prior to Start
        foreach (DictionaryEntry weapon in playerWeaponInventory)
        {
            GameObject disableWeapon = weapon.Value;
            weapon.Value.SetActive(false);
        }

        EquipDefaultWeapon();
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
        EquipHangun();
    }

    void EquipHangun()
    {
        if (inputManager.SelectHandgun())
        {
            SwapWeapon("PlayerHandgun");
        }
    }
    void EquipAssaultRifle()
    {
        if (inputManager.SelectAssaultRifle())
        {
            SwapWeapon("PlayerAssaultRifle");
        }
    }

    void EquipShotgun()
    {
        if (inputManager.SelectShotgun())
        {
            SwapWeapon("PlayerShotgun");
        }
    }

    void SwapWeapon(string selectedWeaponKey)
    {
        if(playerWeaponInventory.ContainsKey(selectedWeaponKey)){
            PlayerFirearm Weapon = (PlayerFirearm) playerWeaponInventory[selectedWeaponKey];
            if (Weapon.inInventory == false)
            {
                Debug.Log("Weapon not currently in inventory"); //display this to player
            }
            //check if swapping weapon is different than the one currently equipped
            else if(selectedWeaponKey != currentWeaponKey) {
                GameObject weaponToSwap = (GameObject) playerWeaponInventory[currentWeaponKey];
                weaponToSwap.SetActive(false);
                GameObject weaponToSwapIn = (GameObject) playerWeaponInventory[selectedWeaponKey];
                weaponToSwapIn.SetActive(true);
                currentWeaponKey = selectedWeaponKey;
            }
            else
            {
                Debug.Log("Already holding weapon"); //solely for debugging
            }
        }
    }
}