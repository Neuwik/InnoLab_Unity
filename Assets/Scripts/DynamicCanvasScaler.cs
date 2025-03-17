using UnityEngine;
using UnityEngine.UI;

public class DynamicCanvasScaler : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    void Start()
    {
        // Hole den CanvasScaler vom aktuellen Canvas
        canvasScaler = GetComponent<CanvasScaler>();

        if (canvasScaler == null)
        {
            Debug.LogError("Kein CanvasScaler gefunden! Füge dieses Script an den Canvas an.");
        }
    }

    void Update()
    {
        if (canvasScaler != null)
        {
            // Aktuelle Fenstergröße auslesen
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Setze die Reference Resolution auf die aktuelle Fenstergröße
            canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);
        }
    }
}
