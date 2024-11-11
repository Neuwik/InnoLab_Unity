using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public GameObject consoleTextObject;
    public Transform consoleContent;

    private TMP_Text LogCreation(string status, string sender, string message)
    {
        if (consoleTextObject == null || consoleContent == null)
        {
            Debug.LogError("TextObject or Content parent not assigned.");
            return null;
        }

        GameObject newTextObject = Instantiate(consoleTextObject, consoleContent);
        TMP_Text textComponent = newTextObject.transform.GetChild(0).GetComponent<TMP_Text>();

        if (textComponent != null)
        {
            textComponent.text = $"{sender}-{status}: {message}";
            return textComponent;
        }
        else
        {
            Debug.LogError("TMP_Text component not found on the child of TextObject.");
            return null;
        }
    }

    public void Log(string status, string sender, string message)
    {
        TMP_Text textComponent = LogCreation(status, sender, message);
        textComponent.color = Color.white;
    }

    public void LogWarning(string status, string sender, string message)
    {
        TMP_Text textComponent = LogCreation(status, sender, message);
        textComponent.color = Color.yellow;
    }

    public void LogError(string status, string sender, string message)
    {
        TMP_Text textComponent = LogCreation(status, sender, message);
        textComponent.color = Color.red;
    }

    public void ClearConsole()
    {
        foreach (Transform child in consoleContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnDestroy()
    {
        ClearConsole();
    }
}
