using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class ShotgunPickup : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        //place shotgun into player inventory/set player shotgun to active?

        //whatever interacts with this - reach it, check what class it is, grab it's playerweaponsmanager
        //set the weapon (shotgun) to isActive = true ?

        //how do you find the player? -  do you track the player with a global value? reach the player through
        //a global?
        Debug.Log("Shotgun Interact Test");
        
        gameObject.SetActive(false);
    }
}