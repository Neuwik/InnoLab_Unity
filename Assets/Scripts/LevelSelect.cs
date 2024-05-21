using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public List<LevelSelectPrefab> LevelPrefabs;

    private void Start()
    {
        LoadAndSetLevelData();
    }

    private void LoadAndSetLevelData()
    {
        var saveDataList = SaveManager.Instance.LoadLevels();

        foreach (var levelPrefab in LevelPrefabs)
        {
            // Check if the index is within the range of save data list
            if (levelPrefab.index >= 0 && levelPrefab.index < saveDataList.Count)
            {
                LevelSaveData saveData = saveDataList[levelPrefab.index];
                if (saveData != null)
                {
                    levelPrefab.SetSaveData(saveData);
                }
                else
                {
                    Debug.LogWarning($"No save data found for level index: {levelPrefab.index}");
                }
            }
            else
            {
                Debug.LogError($"Level index {levelPrefab.index} is out of range for save data list.");
            }
        }
    }
}
