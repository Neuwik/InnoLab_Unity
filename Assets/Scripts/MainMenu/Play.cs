using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private static string _startStamp = "";
    public void OnClickPlayButton()
    {
        _startStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
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

    public void OnClickResetButton() 
    {
        string timestamp = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}";
        //LevelSelect.Instance.LoadAndSetLevelData();
        UMLSaveSystem.DeleteSaves(timestamp);
        try
        {
            System.IO.File.Move(Application.persistentDataPath + "/save.txt", Application.persistentDataPath + $"/save_{_startStamp}-{timestamp}_END.txt");
        }
        catch (Exception e) { }
        /*
        if (SaveManager.Instance != null) 
        {
            Debug.Log("SaveManager.Instance.Init();");
            SaveManager.Instance.Init();
        }
        if (LevelSelect.Instance != null)
        {
            Debug.Log("LevelSelect.Instance.LoadAndSetLevelData();");
            LevelSelect.Instance.LoadAndSetLevelData();
        }
        */
    }
}
