using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource soundEffectAudioSource;

    // Audio clips
    public AudioClip fireDamageSound;
    public AudioClip waterSplashSound;
    public AudioClip garbageCollectSound;
    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject); //keep AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayFireDamageSound()
    {
        //PlayerHealth.cs line 95
        soundEffectAudioSource.PlayOneShot(fireDamageSound);
    }

    public void PlayWaterSplashSound()
    {
        //PlayerHealth.cs line 89
        soundEffectAudioSource.PlayOneShot(waterSplashSound);
    }

    public void PlayerGarbageCollectSound()
    {
        //GarbageCollecter.cs line 45
        soundEffectAudioSource.PlayOneShot(garbageCollectSound);
    }

    public void PlayerBatteryCollectSound()
    {
        //Battery.cs line 117
        //soundEffectAudioSource.PlayOneShot(garbageCollectSound);
    }
}
