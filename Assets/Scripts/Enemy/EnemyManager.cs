using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            return _instance;
        }
    }
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SingletonCheck();
    }

    void SingletonCheck()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}