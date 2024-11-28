using UnityEngine;
using UnityEngine.UI;

public class CanvasZoom : MonoBehaviour
{
    public RectTransform canvasTransform; // Assign your Canvas's RectTransform
    public float zoomSpeed = 0.1f; // Speed of zooming
    public float minScale = 0.5f; // Minimum scale
    public float maxScale = 2.0f; // Maximum scale

    public void ZoomIn()
    {
        Zoom(zoomSpeed);
    }

    public void ZoomOut()
    {
        Zoom(-zoomSpeed);
    }

    private void Zoom(float increment)
    {
        Vector3 newScale = canvasTransform.localScale + Vector3.one * increment;
        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        canvasTransform.localScale = newScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.Equals)) // "+" key
            {
                Zoom(zoomSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.Minus)) // "-" key
            {
                Zoom(-zoomSpeed);
            }
        }
    }

}
