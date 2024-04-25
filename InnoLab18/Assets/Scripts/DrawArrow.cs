using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DrawArrow : MonoBehaviour//, IPointerClickHandler
{
    public bool TargetFound;

    private RectTransform _upperVerticleShaftRectT;
    public GameObject UpperVerticleShaft;

    private RectTransform _lowerVerticleShaftRectT;
    public GameObject LowerVerticleShaft;

    private RectTransform _horizontalVerticleShaftRectT;
    public GameObject HorizontalShaft;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    public Vector2 Startpos;

    // TODO: reddraw
    private GameObject _targetElem;
    public GameObject TargetElem 
    { 
        get { return _targetElem; } 
        set 
        { 
            _targetElem = value;
            gameObject.transform.parent.GetComponent<DragDrop>().OnPossitionChanged.AddListener(SetEnabled);
            _targetElem.GetComponent<DragDrop>().OnPossitionChanged.AddListener(SetEnabled);
            _targetElem.GetComponent<DragDrop>().OnDelete.AddListener(TargetDestroyed);
        }
    }

    private void TargetDestroyed()
    {
        Destroy(gameObject);
    }

    private void SetEnabled()
    {
        enabled = true;
    }
    // TODO: reddraw

    void Start()
    {
        _upperVerticleShaftRectT = UpperVerticleShaft.GetComponent<RectTransform>();
        _lowerVerticleShaftRectT = LowerVerticleShaft.GetComponent<RectTransform>();
        _horizontalVerticleShaftRectT = HorizontalShaft.GetComponent<RectTransform>();
        _arrowHeadRectT = ArrowHead.GetComponent<RectTransform>();
    }

    void Update()
    {
        Debug.Log("UPDATE");
        if (TargetElem == null)
        {
            drawArrow(Input.mousePosition);
            return;
        }
        else
        {
            var _ = TargetElem.GetComponent<RectTransform>().rect;
            Startpos = (Vector2)gameObject.transform.position - new Vector2(0, _.height/2);
            drawArrow((Vector2) TargetElem.transform.position + new Vector2(_.width / 2, _.height));
        }
        if(!GameManager.Instance.ReDrawArrow)
            enabled = false;
    }

    private void drawArrow(Vector2 targetPoint)
    {
        float verticleLength = (targetPoint.y - Startpos.y) / 2;
        float horizontalLength = targetPoint.x - Startpos.x;

        _upperVerticleShaftRectT.position = new Vector2(Startpos.x, Startpos.y + verticleLength / 2);
        _upperVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

        _horizontalVerticleShaftRectT.position = new Vector2(Startpos.x + horizontalLength / 2, Startpos.y + verticleLength);
        _horizontalVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + _upperVerticleShaftRectT.rect.width);

        _lowerVerticleShaftRectT.position = targetPoint + new Vector2(0, - verticleLength / 2);
        _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

        _arrowHeadRectT.position = targetPoint;
    }
    

    /*public void OnPointerClick(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Debug.Log("Ray-Targets:");
        foreach (var ray in results)
        {
            Debug.Log("\t" + ray.gameObject.name);
        }

        if ((TargetElem = results.FindLast(r => r.gameObject.layer == gameObject.layer).gameObject) != null)
        {
            TargetFound = true;
            drawArrow((Vector2) TargetElem.transform.position);
            Debug.Log("\t" + TargetElem.gameObject.name);
        }
    }*/
}
