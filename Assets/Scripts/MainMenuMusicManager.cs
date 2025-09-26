using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusicManager : MonoBehaviour
{
    private static MainMenuMusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; 
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameScene =
            scene.name.Contains("Tutorial") ||
            (scene.name.Contains("Level") && scene.name != "LevelSelect");

        if (isGameScene)
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
        else
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
    }
}
