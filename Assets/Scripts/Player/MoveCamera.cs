using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    void Awake()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
