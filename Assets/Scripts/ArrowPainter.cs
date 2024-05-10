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

public class ArrowPainter : MonoBehaviour, IPointerClickHandler
{
    private RectTransform _upperVerticleShaftRectT;
    public GameObject UpperVerticleShaft;

    private RectTransform _lowerVerticleShaftRectT;
    public GameObject LowerVerticleShaft;

    private RectTransform _horizontalVerticleShaftRectT;
    public GameObject HorizontalShaft;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    public Vector2 Startpos;
    private Vector2 mouseOffset = new Vector2(-5, 5); //  3px does not work, it autosnaps the mouse click???

    private GameObject _targetElem;
    public GameObject TargetElem 
    { 
        get { return _targetElem; } 
        set 
        { 
            _targetElem = value;
            gameObject.transform.parent.GetComponent<DragDrop>()?.OnPossitionChanged.AddListener(SetEnabled);
            _targetElem.GetComponent<DragDrop>().OnPossitionChanged.AddListener(SetEnabled);
            _targetElem.GetComponent<DragDrop>().OnDelete.AddListener(TargetDestroyed);

            AUMLElement prev = transform.parent.GetComponent<AUMLElement>();
            bool conditional = false; //muss true sein, wenn man einen Pfeil für Condition == false zeichenen möchte
            prev?.ChangeNextAction(_targetElem.GetComponent<AUMLElement>(), conditional);
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

    void Start()
    {
        _upperVerticleShaftRectT = UpperVerticleShaft.GetComponent<RectTransform>();
        _lowerVerticleShaftRectT = LowerVerticleShaft.GetComponent<RectTransform>();
        _horizontalVerticleShaftRectT = HorizontalShaft.GetComponent<RectTransform>();
        _arrowHeadRectT = ArrowHead.GetComponent<RectTransform>();
    }

    void Update()
    {
        //Debug.Log("UPDATE");
        if (TargetElem == null)
        {
            DrawArrow((Vector2)Input.mousePosition + mouseOffset);
            return;
        }
        else
        {
            var _ = TargetElem.GetComponent<RectTransform>().rect;
            Startpos = (Vector2) gameObject.transform.position - new Vector2(0, gameObject.transform.parent.GetComponent<RectTransform>().rect.height/2);
            DrawArrow((Vector2) TargetElem.transform.position + new Vector2(_.width / 2, _.height));
        }
        if(!GameManager.Instance.ReDrawArrow)
            enabled = false;
    }
    // TODO:
    //       if mouse howers above object set bool isHowering to 1 and position to target pos 
    //       change to 2 horizontal- and 1 vertical arrows if mousePos above startPos.
    //  done limit to one arrow per element execept if-blocks -> 2 
    //       startPos's for if-blocks -> 2 sider arrows, 1 side arrow and 1 bottom arrow
    private void DrawArrow(Vector2 targetPoint)
    {
        
        /*if ()
        {

        }*/
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
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left: // change true or false if arrow starts in conditionblock

                return;

            case PointerEventData.InputButton.Right: // reattach Arrow

                return;

            case PointerEventData.InputButton.Middle: // destroy Arrow
                //ReduceTargetAmount();
                Destroy(GameManager.Instance.ActiveArrow);
                return;
        }
        
    }
}
