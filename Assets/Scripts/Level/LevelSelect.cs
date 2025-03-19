using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private List<LevelSelectPrefab> LevelPrefabs;

    [SerializeField]
    private List<int> _levelSceneIndexes = new List<int>();
    public int GetLevelSceneIndexByLevelNumber(int num)
    {
        if (num < 1 || _levelSceneIndexes.Count < num)
        {
            Debug.LogError($"Level with Number {num} does not exist in LevelSelect Manager.");
            return -1;
        }
        return _levelSceneIndexes[num - 1];
    }

    private void Start()
    {
        LevelPrefabs = FindObjectsOfType<LevelSelectPrefab>(true).OrderBy(l => l.index).ToList();

        LoadAndSetLevelData();
    }

    public void LoadAndSetLevelData()
    {
        var saveDataList = SaveManager.Instance.LoadLevels();

        foreach (var levelPrefab in LevelPrefabs)
        {
            // Check if the index is within the range of save data list
            if (levelPrefab.index >= 0 && levelPrefab.index <= saveDataList.Count + 1)
            {
                if (levelPrefab.index == saveDataList.Count + 1)
                {
                    levelPrefab.SetUnlocked();
                    Debug.Log($"Next unlocked Level: {levelPrefab.index}");
                    return;
                }

                LevelSaveData saveData = saveDataList[levelPrefab.index - 1];
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
                // Has not been saved yet / was never played
                Debug.Log($"Level index {levelPrefab.index} is out of range for save data list.");
            }
        }
    }
}
