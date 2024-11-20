using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider VolumeSlider;
    public Button MenuButton;
    public Button ExitButton;
    public int levelSelectSceneIndex;

    void Start()
    {
        // Assign button functionalities
        MenuButton.onClick.AddListener(LoadMenuScene);
        ExitButton.onClick.AddListener(QuitGame);
        VolumeSlider.onValueChanged.AddListener(HandleMasterVolume);

        // Initialize slider with current volume
        VolumeSlider.value = AudioListener.volume;
    }

    public void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
    }

    public void LoadMenuScene()
    {
        // Resume time before loading
        Time.timeScale = 1f;

        // Load the specified scene
        GameManager.Instance.LevelManager.LoadLevelByIndex(levelSelectSceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void HandleMasterVolume(float volume)
    {
        // Adjust the master volume
        AudioListener.volume = volume;
    }
}
