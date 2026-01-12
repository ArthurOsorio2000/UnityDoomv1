using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectObject;
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }

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

    public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);
        //assign the audioclip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
