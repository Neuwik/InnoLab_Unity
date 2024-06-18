using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ArrowPainter : MonoBehaviour
{
    private RectTransform _upperVerticleShaftRectT;
    public GameObject UpperVerticleShaft;

    private RectTransform _lowerVerticleShaftRectT;
    public GameObject LowerVerticleShaft;

    private Vector3 _ConditionalOffset;
    private RectTransform _lowerHorizontalShaftRectT;
    public GameObject LowerHorizontalShaft;

    private RectTransform _upperHorizontalShaftRectT;
    public GameObject UpperHorizontalShaft;

    private RectTransform _arrowHeadRectT;
    public GameObject ArrowHead;

    private RectTransform _conditionTextRectT;
    public GameObject ConditionText;

    public Vector2 StartPos;
    private Vector2 mouseOffset = new Vector2(-5, 5); //  3px does not work, it autosnaps the mouse click???

    private bool _conditionOutcome;
    public bool ConditionOutcome { get { return _conditionOutcome; } set { _conditionOutcome = value; } }

    private GameObject _parentElem;
    private Rect _parentRect;

    private GameObject _targetElem;
    private Rect _targetRect;

    private AUMLElement _prev;


    private CreateArrow _prevCreateArrow;

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

            _prev = transform.parent.GetComponent<AUMLElement>();
            CreateArrow CA = _targetElem.GetComponent<CreateArrow>();
            CA.OnDelete.AddListener(TargetDestroyed);

            //muss true sein, wenn man einen Pfeil für Condition == false zeichenen möchte
            _prevCreateArrow = _prev.GetComponent<CreateArrow>();
            bool conditional = _prevCreateArrow.TargetMaxAmount > 1 && _prevCreateArrow.TargetAmount > 1;

            _prev?.ChangeNextAction(_targetElem.GetComponent<AUMLElement>(), conditional);
        }
    }
    private void TargetDestroyed()
    {
        _prevCreateArrow.ReduceTargetAmount();
        Destroy(gameObject);
    }

    private void SetEnabled()
    {
        enabled = true;
    }
    void Start()
    {
        _parentElem = gameObject.transform.parent.gameObject;
        _parentRect = _parentElem.GetComponent<RectTransform>().rect;
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
            //  +-> if mouse howers above object set bool isHowering to 1 and StartPos to target pos of hovered elem
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
        float targetHalfWidth = TargetElem != null ? TargetElem.GetComponent<RectTransform>().rect.width / 2 : 0;

        StartPos = (Vector2) gameObject.transform.position;
        if (targetPoint.y < StartPos.y - _parentRect.height / 2)
        {
            StartPos -= new Vector2(0, _parentRect.height / 2);
            if (UpperHorizontalShaft.activeSelf)
            {
                UpperHorizontalShaft.SetActive(false);
                UpperVerticleShaft.SetActive(true);
            }
            if(TargetElem != null)
            {
                targetPoint += new Vector2(0, TargetElem.GetComponent<RectTransform>().rect.height / 2);
            }

            verticleLength = (targetPoint.y - StartPos.y) / 2;
            horizontalLength = targetPoint.x - StartPos.x;
            shaftOffset = _upperVerticleShaftRectT.rect.width;
            _ConditionalOffset = new Vector3(0, 7, 0);

            _upperVerticleShaftRectT.position = new Vector2(StartPos.x, StartPos.y + verticleLength / 2);
            _upperVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _lowerHorizontalShaftRectT.position = new Vector2(StartPos.x + horizontalLength / 2, StartPos.y + verticleLength);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength) + shaftOffset);

            _conditionTextRectT.position = _lowerHorizontalShaftRectT.position + _ConditionalOffset;

            _lowerVerticleShaftRectT.position = targetPoint - new Vector2(0, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _arrowHeadRectT.position = targetPoint;
            _arrowHeadRectT.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if(StartPos.x + _parentRect.width / 2 >= targetPoint.x - (TargetElem != null ? targetHalfWidth : 50) &&
                StartPos.x - _parentRect.width / 2 <= targetPoint.x + (TargetElem != null ? targetHalfWidth : 50) ||
                StartPos.x + _parentRect.width / 2 >= targetPoint.x + (TargetElem != null ? targetHalfWidth : 50) &&
                StartPos.x - _parentRect.width / 2 <= targetPoint.x - (TargetElem != null ? targetHalfWidth : 50) )
        {
            if (UpperVerticleShaft.activeSelf)
            {
                UpperVerticleShaft.SetActive(false);
                UpperHorizontalShaft.SetActive(true);
            }

            _ConditionalOffset = new Vector3(15, 0, 0);
            shaftOffset = _lowerVerticleShaftRectT.rect.width;
            verticleLength = _parentRect.height / 2 + targetPoint.y - StartPos.y;

            float directionHelper = 1;
            float upperLength;

            if (StartPos.x - _parentRect.width / 2 <= targetPoint.x &&
                targetPoint.x <= StartPos.x ||
                targetPoint.x - targetHalfWidth >= StartPos.x - _parentRect.width / 2)
            {
                StartPos -= new Vector2((_parentRect.width / 2) - 5, _parentRect.height / 2);
                _arrowHeadRectT.rotation = Quaternion.Euler(0f, 0f, 90f);
                horizontalLength = (targetPoint.x - StartPos.x) / 2;
                if (TargetElem != null)
                {
                    targetPoint.x -= targetHalfWidth;
                }
                upperLength = _lowerVerticleShaftRectT.position.x - targetPoint.x;
                directionHelper = -1;
            }
            else
            {
                StartPos += new Vector2((_parentRect.width / 2) - 5, -_parentRect.height / 2);
                _arrowHeadRectT.rotation = Quaternion.Euler(0f, 0f, -90f);
                horizontalLength = (StartPos.x - targetPoint.x) / 2;

                if (TargetElem != null)
                {
                    targetPoint.x += targetHalfWidth;
                }
                upperLength = _lowerVerticleShaftRectT.position.x - targetPoint.x;
            }

            _lowerHorizontalShaftRectT.position = StartPos + new Vector2(horizontalLength / 3 * directionHelper, 0);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength / 1.5f + shaftOffset));

            _conditionTextRectT.position = _lowerVerticleShaftRectT.position + _ConditionalOffset * directionHelper * -1; // don't judge :D
            
            
            _lowerVerticleShaftRectT.position = StartPos + new Vector2(horizontalLength / 1.5f * directionHelper, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _upperHorizontalShaftRectT.position = targetPoint + new Vector2(upperLength / 2, 0);
            _upperHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(upperLength) + shaftOffset);

            _arrowHeadRectT.position = targetPoint;
        }
        else // upwards arrow where elements have a large distance in between them
        {
            if (UpperVerticleShaft.activeSelf)
            {
                UpperVerticleShaft.SetActive(false);
                UpperHorizontalShaft.SetActive(true);
            }

            _ConditionalOffset = new Vector3(15, 0, 0);
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
                _ConditionalOffset *= -1;
                if (TargetElem != null)
                {
                    targetPoint -= new Vector2(TargetElem.GetComponent<RectTransform>().rect.width / 2, 0);
                }
            }
            

            verticleLength = targetPoint.y - StartPos.y;
            horizontalLength = (targetPoint.x - StartPos.x) / 2;
            
            _lowerHorizontalShaftRectT.position = StartPos + new Vector2(horizontalLength / 2, 0);
            _lowerHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength + shaftOffset));

            _conditionTextRectT.position = _lowerVerticleShaftRectT.position + _ConditionalOffset;

            _lowerVerticleShaftRectT.position = StartPos + new Vector2(horizontalLength, verticleLength / 2);
            _lowerVerticleShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Abs(verticleLength));

            _upperHorizontalShaftRectT.position = targetPoint - new Vector2((horizontalLength + shaftOffset) / 2, 0);
            _upperHorizontalShaftRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Math.Abs(horizontalLength));

            _arrowHeadRectT.position = targetPoint;
        }
    }
}
