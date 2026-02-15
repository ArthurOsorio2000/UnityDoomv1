using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunPickup : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Shotgun Interact Test");
        gameObject.SetActive(false);
    }
}
