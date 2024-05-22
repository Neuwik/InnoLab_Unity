using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;


public class LevelManager : MonoBehaviour
{
    private string sceneName;
    public SceneAsset sceneAsset;

    void OnValidate()
    {
        if (sceneAsset != null)
            sceneName = sceneAsset.name;
    }

    public void LoadNextLevel()
    {
        /*
         Add Scenes to Build Settings: First, ensure that all scenes are added to your project’s build settings.
            Go to File > Build Settings.
            Drag and drop your scenes into the window under "Scenes In Build" or use the "Add Open Scenes" button to add the currently open scene. 
         */
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadLevelByName()
    {
        if(!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelByIndex(int index)
    {
        if (index >= 0)
            SceneManager.LoadScene(index);
    }
}