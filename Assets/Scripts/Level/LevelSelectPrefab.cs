using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPrefab : MonoBehaviour
{
    public int index;

    public GameObject IndexGO;
    public GameObject Lock;
    public GameObject GreyCover;

    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public GameObject EmptyStar1;
    public GameObject EmptyStar2;
    public GameObject EmptyStar3;

    private void Awake()
    {
        IndexGO.GetComponent<TextMeshProUGUI>().text = index.ToString();
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.LevelManager.LoadLevelByIndex(LevelSelect.Instance.GetLevelSceneIndexByLevelNumber(index)));
    }

    public void SetUnlocked()
    {
        // show
        IndexGO.SetActive(true);
        EmptyStar1.SetActive(true);
        EmptyStar2.SetActive(true);
        EmptyStar3.SetActive(true);

        // hide
        Lock.SetActive(false);
        GreyCover.SetActive(false);

        GetComponent<Button>().enabled = true;
    }

    public void SetSaveData(LevelSaveData saveData)
    {
        SetUnlocked();
        if (saveData.starsEarned >= 1)
            Star1.SetActive(true);
        if (saveData.starsEarned >= 2)
            Star2.SetActive(true);
        if (saveData.starsEarned >= 3)
            Star3.SetActive(true);
    }
}
