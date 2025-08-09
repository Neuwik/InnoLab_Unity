using Assets.Scripts.Global;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private static string START_STAMP = "";
    public void OnClickPlayButton()
    {
        START_STAMP = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
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

    public void OnClickChangeControllGroupButton()
    {
        ControlGroup.IS_CONTROLL_GROUP_B = !ControlGroup.IS_CONTROLL_GROUP_B;
        transform.Find("ChangeControllGroupButton").GetComponentInChildren<TextMeshProUGUI>().text = ControlGroup.IS_CONTROLL_GROUP_B ? "B" : "C";
    }
    public void OnClickResetButton() 
    {
        string timestamp = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}";
        //LevelSelect.Instance.LoadAndSetLevelData();
        UMLSaveSystem.DeleteSaves(timestamp);
        try
        {
            System.IO.File.Move(Application.persistentDataPath + "/save.txt", Application.persistentDataPath + $"/save_{START_STAMP}-{timestamp}_END.txt");
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
