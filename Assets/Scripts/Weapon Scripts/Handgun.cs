using UnityEngine;


//I could make a generic weapon class as a parent, and inherit it for each individual weapon?
public class Handgun : MonoBehaviour
{
    [SerializeField] private AudioClip handGun;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    private InputManager inputManager;
    public Camera playerCamera;

    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        AudioManager.Instance.PlaySoundEffect(handGun, transform, 1f);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
