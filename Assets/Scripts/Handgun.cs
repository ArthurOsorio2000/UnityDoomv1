using Unity.VisualScripting;
using UnityEngine;

public class Handgun : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] AudioSource handgunFire;

    void Start()
    {
        inputManager = InputManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        if (inputManager.PlayerShooting())
        {
            
        }
    }
}
