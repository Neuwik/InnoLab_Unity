using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public GameObject tutorialBoxPrefab;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToggleTutorialBox);
    }

    public void ToggleTutorialBox()
    {
        if (tutorialBoxPrefab.activeSelf)
        {
            tutorialBoxPrefab.SetActive(false);
            return;
        }

        tutorialBoxPrefab.SetActive(true);
    }
}
