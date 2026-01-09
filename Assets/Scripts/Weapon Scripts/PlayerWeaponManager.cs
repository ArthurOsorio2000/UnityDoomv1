using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int selectedWeapon = 0;
    [SerializeField] protected InputManager inputManager;
    void Start()
    {
        //assign common components
        inputManager = InputManager.Instance;
        SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // switch ()
        // {
            
        // }
    }

    void DeactivateNestedWeapons()
    {
        //go through all game objects nested in the player weapon manager and turn them off
        foreach (Transform weapon in transform)
            {
                weapon.gameObject.SetActive(false);
            }
    }
}