using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private int currentWeapon;

    //note: this holds all weapons. if you want a fixed spot for them, import them and sort them based on an id?
    //for dynamic pickup populate based on other factors like slot availability or queue recency
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private InputManager inputManager;
    void Start()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        //assign common components
        inputManager = InputManager.Instance;
        EquipDefaultWeapon();
        weapons[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        EquipHangun();
        EquipAssaultRifle();
    }

    //list of currently equipped weapons.
    //if the player has not yet acquired the weapon - the player cannot use it

    //once the player has the weapon and it is not currently equipped, stow the
    //currently equipped weapon and unhide the selected one

    //for testing - for now, let's assume the player currently has both the handgun and assaultrifle

    //will each gun gameobject have a script, and when their input action get command is called,
    //they activate a method within this manager to activate itself?
    //no, but what if they aren't activated?
    //this means the behaviours of each gun are defined in their objects, but the equipping and unequipping
    //(loading and deloading) are done here?

    //using a switch isn't going to work as it takes an input. should something in update be constantly
    //polling to see if I've pressed any weapon select buttons?

    //if the handgun's been pressed (and is in inventory) - deload everything and load handgun?
    //if the assault rifle's been pressed - deload everything and load ar?
    //1: check for weapon and note down index in list - and check if isAquired
    //2. deload all weapons(?) or deload currently loaded weapon?
    //3. load selected weapon

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
    

    void SwapWeapon(int selectedWeapon)
    {
        if(selectedWeapon != currentWeapon) //here, when implemented, also check if the weapon has been collected, yet
            {
                weapons[currentWeapon].SetActive(false);
                weapons[selectedWeapon].SetActive(true);
                currentWeapon = selectedWeapon;
            }
    }
}