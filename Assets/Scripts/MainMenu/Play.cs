using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnClickSettingsButton()
    {
        //SceneManager.LoadScene("Settings");
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
