using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawArrow : MonoBehaviour, IPointerClickHandler
{
    private bool targetFound;
    void Start()
    {
        targetFound = false;
        drawArrow();
    }

    void Update()
    {
        
    }
    private void drawArrow()
    {
        //while (!targetFound)
        {

        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 clickPos = eventData.position;
        var _ = clickPos + new Vector3(0, 0, 15);
        Physics.Raycast(clickPos, _, out var hit, 15, LayerMask.GetMask("UI"));
        //if (hit.) ;
    }
}
