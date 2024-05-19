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

    private readonly Vector3 _lhsOffset = new Vector3(0, 7, 0);
    private RectTransform _lowerHorizontalShaftRectT;
    public GameObject LowerHorizontalShaft;

    private RectTransform _upperHorizontalShaftRectT;
    public GameObject UpperHorizontalShaft;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    private RectTransform _conditionTextRectT;
    public GameObject ConditionText;

    private Rect _parentRect;

    public Vector2 StartPos;
    private Vector2 mouseOffset = new Vector2(-5, 5); //  3px does not work, it autosnaps the mouse click???

    private bool _conditionOutcome;
    public bool ConditionOutcome { get { return _conditionOutcome; } set { _conditionOutcome = value; } }
    private GameObject _targetElem;
    private Rect _targetRect;
    public GameObject TargetElem 
    { 
        get { return _targetElem; } 
        set 
        { 
            _targetElem = value;
            _targetRect = GetComponent<RectTransform>().rect;
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
        _parentRect = gameObject.transform.parent.GetComponent<RectTransform>().rect;
        _upperVerticleShaftRectT = UpperVerticleShaft.GetComponent<RectTransform>();
        _lowerVerticleShaftRectT = LowerVerticleShaft.GetComponent<RectTransform>();
        _lowerHorizontalShaftRectT = LowerHorizontalShaft.GetComponent<RectTransform>();
        _upperHorizontalShaftRectT = UpperHorizontalShaft.GetComponent<RectTransform>();
        _arrowHeadRectT = ArrowHead.GetComponent<RectTransform>();
        _conditionTextRectT = ConditionText.GetComponent<RectTransform>();
    }

    void Update()
    {
        
        //Debug.Log("UPDATE");
        if (TargetElem == null)
        {
            // TODO:
            //  +-> if mouse howers above object set bool isHowering to 1 and StartPos to target pos 
            DrawArrow((Vector2)Input.mousePosition + mouseOffset);
            return;
        }
        else
        {
            DrawArrow((Vector2) TargetElem.transform.position );
        }
        if(!GameManager.Instance.ReDrawArrow)
            enabled = false;
    }
    
 
    private void DrawArrow(Vector2 targetPoint)
    {
        float shaftOffset;
        float verticleLength;
        float horizontalLength;

        StartPos = (Vector2) gameObject.transform.position;
        if (targetPoint.y < StartPos.y - _parentRect.height / 2)
        {
            if (UpperHorizontalShaft.activeSelf)
            {
                UpperHorizontalShaft.SetActive(false);
                UpperVerticleShaft.SetActive(true);
            }

            StartPos -= new Vector2(0, _parentRect.height / 2);
            verticleLength = (targetPoint.y - StartPos.y) / 2;
            horizontalLength = targetPoint.x - StartPos.x;
            shaftOffset = _upperVerticleShaftRectT.rect.width;

            if (TargetElem != null)
            {
                targetPoint += new Vector2(0, TargetElem.GetComponent<RectTransform>().rect.height / 2);
            }

            _upperVerticleShaftRectT.position = new Vector2(StartPos.x, StartPos.y + verticleLength / 2);
            _upperVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _lowerHorizontalShaftRectT.position = new Vector2(StartPos.x + horizontalLength / 2, StartPos.y + verticleLength);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + shaftOffset);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position + _lhsOffset;

            _lowerVerticleShaftRectT.position = targetPoint - new Vector2(0, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _arrowHeadRectT.position = targetPoint;
            _arrowHeadRectT.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if(StartPos.x + _parentRect.width / 2 <= targetPoint.x - 10 || // upwards arrow where elements have a large distance in between them
                StartPos.x - _parentRect.width / 2 >= targetPoint.x - 10 )
        {
            if (UpperVerticleShaft.activeSelf)
            {
                UpperVerticleShaft.SetActive(false);
                UpperHorizontalShaft.SetActive(true);
            }

            verticleLength = targetPoint.y - StartPos.y;
            horizontalLength = (targetPoint.x - StartPos.x) / 2;
            shaftOffset = _lowerVerticleShaftRectT.rect.width;

            if ((targetPoint.x + (_targetRect != null? _targetRect.width / 2 : 0)) <= StartPos.x)
            {
                StartPos -= new Vector2((_parentRect.width / 2) - 5, _parentRect.height / 2);
                _arrowHeadRectT.rotation = Quaternion.Euler(0f, 0f, -90f);
                shaftOffset *= -1;
                if (TargetElem != null)
                {
                    targetPoint += new Vector2(TargetElem.GetComponent<RectTransform>().rect.width / 2, 0);
                }   
            }
            else
            {
                StartPos += new Vector2((_parentRect.width / 2) - 5, -_parentRect.height / 2);
                _arrowHeadRectT.rotation = Quaternion.Euler(0f, 0f, 90f);

                if (TargetElem != null)
                {
                    targetPoint -= new Vector2(TargetElem.GetComponent<RectTransform>().rect.width / 2, 0);
                }
            }

            _lowerHorizontalShaftRectT.position = StartPos + new Vector2(horizontalLength / 2, 0);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + _lowerVerticleShaftRectT.rect.width);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position + _lhsOffset;

            _lowerVerticleShaftRectT.position = StartPos + new Vector2(horizontalLength, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _upperHorizontalShaftRectT.position = targetPoint - new Vector2((horizontalLength + shaftOffset) / 2, 0);
            _upperHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength));

            _arrowHeadRectT.position = targetPoint;
        }
        /*else
        {
            
            if (UpperVerticleShaft.activeSelf)
            {
                UpperVerticleShaft.SetActive(false);
                UpperHorizontalShaft.SetActive(true);
            }

            verticleLength = targetPoint.y - StartPos.y;
            horizontalLength = (targetPoint.x - StartPos.x) / 2;
            shaftOffset = _lowerVerticleShaftRectT.rect.width;
            
            if(targetPoint.x + _parentRect.width / 2 < )
            _lowerHorizontalShaftRectT.position = StartPos + new Vector2(horizontalLength / 2, 0);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + _lowerVerticleShaftRectT.rect.width);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position + _lhsOffset;

            _lowerVerticleShaftRectT.position = StartPos + new Vector2(horizontalLength, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _upperHorizontalShaftRectT.position = targetPoint - new Vector2((horizontalLength + shaftOffset) / 2, 0);
            _upperHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength));
        }
        */

    }
}
