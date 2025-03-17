using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusicManager : MonoBehaviour
{
    private static MainMenuMusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Tutorial") || scene.name.Contains("Level"))
        {
            if (audioSource.isPlaying && !scene.name.Equals("LevelSelect")) //Except LevelSelect
            {
                audioSource.Stop();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
