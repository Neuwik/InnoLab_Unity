using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DrawArrow : MonoBehaviour//, IPointerClickHandler
{ 
    private bool _targetFound;

    private RectTransform _upperVerticleShaftRectT;
    public GameObject UpperVerticleShaft;

    private RectTransform _lowerVerticleShaftRectT;
    public GameObject LowerVerticleShaft;

    private RectTransform _horizontalVerticleShaftRectT;
    public GameObject HorizontalShaft;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    public Vector2 Startpos;
    void Start()
    {
        _upperVerticleShaftRectT = UpperVerticleShaft.GetComponent<RectTransform>();
        _lowerVerticleShaftRectT = LowerVerticleShaft.GetComponent<RectTransform>();
        _horizontalVerticleShaftRectT = HorizontalShaft.GetComponent<RectTransform>();
        _arrowHeadRectT = ArrowHead.GetComponent<RectTransform>();
        _targetFound = false;
    }

    void Update()
    {
        if (!_targetFound)
        {
            drawArrow(); 
            return;
        }
        enabled = false;
    }
    private void drawArrow()
    {
        float arrowGirth = 10;
        Vector2 mousePos =  Input.mousePosition;
        Vector2 verticleLength = new Vector2(arrowGirth, (Math.Abs(mousePos.y - Startpos.y)) / 2);
        Vector2 horizontalLength = new Vector2(Math.Abs(mousePos.x - Startpos.x), arrowGirth);
        //float verticleLength = (mousePos.y - Startpos.y) / 2;
        _upperVerticleShaftRectT.sizeDelta = verticleLength;
        _upperVerticleShaftRectT.position = new Vector2(Startpos.x, verticleLength.y/2);

        _horizontalVerticleShaftRectT.position = new Vector2 (Startpos.x, Startpos.y + verticleLength.y);
        _horizontalVerticleShaftRectT.sizeDelta = horizontalLength;

        _lowerVerticleShaftRectT.position = new Vector2 (mousePos.x, _horizontalVerticleShaftRectT.position.y);
        _lowerVerticleShaftRectT.sizeDelta = verticleLength;

        _arrowHeadRectT.position = mousePos;



    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        *//* Vector3 clickPos = eventData.position;
         var _ = clickPos + new Vector3(0, 0, 15);
         Physics.Raycast(clickPos, _, out var hit, 15, LayerMask.GetMask("UI"));*//*
        //if (hit.) ;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("hit");
        }
    }*/
}
