using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject PauseMenuPrefab; // Reference to the pause menu prefab

    private PauseMenu _pauseMenu;

    void Start()
    {
        // Get the PauseMenu script from the prefab
        _pauseMenu = PauseMenuPrefab.GetComponent<PauseMenu>();

        // Ensure the menu starts hidden
        PauseMenuPrefab.SetActive(false);
    }

    void Update()
    {
        // Toggle the pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (PauseMenuPrefab.activeSelf)
        {
            // Hide the menu and resume the game
            PauseMenuPrefab.SetActive(false);
            _pauseMenu.ResumeGame();
            return;
        }

        // Show the menu and pause the game
        PauseMenuPrefab.SetActive(true);
        _pauseMenu.PauseGame();
    }
}
