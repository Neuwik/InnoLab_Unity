using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public int levelNumber;
    public int starsEarned;
    public int stepsTaken;
    public int umlElementsUsed;
    public float successQualityPercent;
    public float percentHealth;
    public float percentEnergy;
}

[System.Serializable]
public class LevelSaveDataList
{
    public List<LevelSaveData> levelSaveDataList = new List<LevelSaveData>();
}

public class SaveManager : MonoBehaviour
{
    #region SINGLETON

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Init();
        }
    }

    #endregion

    private string saveFilePath;
    private LevelSaveDataList levelSaveDataList = new LevelSaveDataList();

    private void Init()
    {
        try
        {
            Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

            // Initialize save file path
            saveFilePath = Application.persistentDataPath + "/save.txt";

            // Test if Save Folder exists
            string directoryPath = Path.GetDirectoryName(saveFilePath);
            if (!Directory.Exists(directoryPath))
            {
                // Create Save Folder
                Directory.CreateDirectory(directoryPath);
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to initialize save directory: " + ex.Message);
        }
    }

    #region SAVE and LOAD LEVELS

    public void SaveLevel(LevelSaveData levelData)
    {
        try
        {
            LoadSaveData();
            UpdateLevelData(levelData);
            WriteSaveFile();
            Debug.Log("Level data saved successfully");
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to save level data: " + ex.Message);
        }
    }

    public List<LevelSaveData> LoadLevels()
    {
        try
        {
            LoadSaveData();
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to load level data: " + ex.Message);
        }
        return levelSaveDataList.levelSaveDataList; // Access the list inside the container
    }

    #endregion

    // Update or add level data
    private void UpdateLevelData(LevelSaveData levelData)
    {
        int index = levelSaveDataList.levelSaveDataList.FindIndex(data => data.levelNumber == levelData.levelNumber);
        if (index != -1)
        {
            if (levelSaveDataList.levelSaveDataList[index].starsEarned <= levelData.starsEarned)
                levelSaveDataList.levelSaveDataList[index] = levelData;
        }
        else
        {
            levelSaveDataList.levelSaveDataList.Add(levelData);
        }
    }

    #region SAVE and LOAD FILE

    private void WriteSaveFile()
    {
        try
        {
            string json = JsonUtility.ToJson(levelSaveDataList);
            File.WriteAllText(saveFilePath, json);
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to write save file: " + ex.Message);
        }
    }

    private void LoadSaveData()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                levelSaveDataList = JsonUtility.FromJson<LevelSaveDataList>(json);
            }
            else
            {
                levelSaveDataList = new LevelSaveDataList();
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to load save data: " + ex.Message);
            levelSaveDataList = new LevelSaveDataList();
        }
    }

    #endregion
}