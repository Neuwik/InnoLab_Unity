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

public class ArrowPainter : MonoBehaviour
{
    private RectTransform _upperVerticleShaftRectT;
    public GameObject UpperVerticleShaft;

    private RectTransform _lowerVerticleShaftRectT;
    public GameObject LowerVerticleShaft;

    private RectTransform _lowerHorizontalShaftRectT;
    public GameObject LowerHorizontalShaft;

    private RectTransform _upperHorizontalShaftRectT;
    public GameObject UpperHorizontalVerticleShaftRectT;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    private RectTransform _conditionTextRectT;
    public GameObject ConditionText;

    public Vector2 Startpos;
    private Vector2 mouseOffset = new Vector2(-5, 5); //  3px does not work, it autosnaps the mouse click???

    private GameObject _targetElem;

    private bool _conditionOutcome;
    public bool ConditionOutcome { get { return _conditionOutcome; } set { _conditionOutcome = value; } }
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
            CreateArrow CA = _targetElem.GetComponent<CreateArrow>();
            CA.OnDelete.AddListener(TargetDestroyed);

            bool conditional = false;

            Debug.Log(CA.TargetAmount);
            if (CA.TargetAmount == 1)
            {
                conditional = true; //muss true sein, wenn man einen Pfeil für Condition == false zeichenen möchte
            }
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
        _lowerHorizontalShaftRectT = LowerHorizontalShaft.GetComponent<RectTransform>();
        _arrowHeadRectT = ArrowHead.GetComponent<RectTransform>();
        _conditionTextRectT = ConditionText.GetComponent<RectTransform>();
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
    //           if mouse howers above object set bool isHowering to 1 and position to target pos 
    //           change to 2 horizontal- and 1 vertical arrows if mousePos above startPos.
    // 1.5h done limit to one arrow per element execept if-blocks -> 2 
    //           startPos's for if-blocks -> 2 sider arrows, 1 side arrow and 1 bottom arrow
    // 2h        add true and false to condition arrows
    // 1h        delete arrows
    private void DrawArrow(Vector2 targetPoint)
    {
        float verticleLength = targetPoint.y - Startpos.y;
        float horizontalLength = targetPoint.x - Startpos.x;

        if (targetPoint.y <= Startpos.y)
        {
            verticleLength /= 2;
            _upperVerticleShaftRectT.position = new Vector2(Startpos.x, Startpos.y + verticleLength / 2);
            _upperVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _lowerHorizontalShaftRectT.position = new Vector2(Startpos.x + horizontalLength / 2, Startpos.y + verticleLength);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + _upperVerticleShaftRectT.rect.width);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position + new Vector3(0, 7, 0);

            _lowerVerticleShaftRectT.position = targetPoint + new Vector2(0, -verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _arrowHeadRectT.position = targetPoint;
        }
        else
        {
            horizontalLength /= 2;
            _upperVerticleShaftRectT.position = new Vector2(Startpos.x, Startpos.y + verticleLength / 2);
            _upperVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _lowerHorizontalShaftRectT.position = new Vector2(Startpos.x + horizontalLength / 2, Startpos.y + verticleLength);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + _upperVerticleShaftRectT.rect.width);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position;

            _lowerVerticleShaftRectT.position = targetPoint + new Vector2(0, -verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _arrowHeadRectT.position = targetPoint;
        }
        
        
    }
}
