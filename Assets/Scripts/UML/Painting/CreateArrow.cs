using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using static UnityEditor.Rendering.FilterWindow;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    private int _maxArrowCount;
    private List<ArrowPainter> _arrows;
    private void AddArrow(ArrowPainter arrow)
    {
        arrow.OnDelete.AddListener(RemoveArrow);
        _arrows.Add(arrow);
    }
    private void RemoveArrow(ArrowPainter arrow)
    {
        _arrows.Remove(arrow);
        arrow?.OnDelete.RemoveListener(RemoveArrow);
    }

    public ArrowPainter ArrowPrefab;

    public UnityEvent OnDelete;
    public bool CanDraw = false;

    private void Start()
    {
        _arrows = new List<ArrowPainter>();
        if (TryGetComponent<AUMLElementTrueFalse>(out _))
        {
            _maxArrowCount = 2;
        }
        else
        {
            _maxArrowCount = 1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanDraw)
        {
            return;
        }
        
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left: // Attach Arrow -> happens on TargetObject
                if (GameManager.Instance.ActiveArrow != null &&
                    gameObject.TryGetComponent<CreateArrow>(out CreateArrow element))
                {
                    if(GameManager.Instance.ActiveArrow.GetComponent<ArrowPainter>().TrySetTargetElem(element))
                        GameManager.Instance.ActiveArrow = null;
                }
                return;

            case PointerEventData.InputButton.Right:

                if (_arrows.Count == 2) // Swtich True and False Arrow
                {
                    foreach (ArrowPainter arrow in _arrows)
                    {
                        arrow.ToggleCondition();
                    }
                    
                    gameObject.GetComponent<AUMLElementTrueFalse>().SwitchNextActions();

                    return;
                }

                if (GameManager.Instance.ActiveArrow == null &&
                    _arrows.Count < _maxArrowCount )
                {
                    ArrowPainter newArrow = Instantiate(ArrowPrefab, gameObject.transform);
                    newArrow.transform.SetAsFirstSibling();
                    AddArrow(newArrow);

                    if (_maxArrowCount == 2) // => only Condition blocks
                    {
                        newArrow.SetCondition(_arrows.Count != 2);
                    }
                    GameManager.Instance.ActiveArrow = newArrow;
                    
                }
                return;

            case PointerEventData.InputButton.Middle: // Delete Arrow
                if (_arrows.Count > 0)
                {
                    ArrowPainter arrow = _arrows.Last();
                    RemoveArrow(arrow);
                    Destroy(arrow.gameObject);
                }
                else
                {
                    OnDelete.Invoke();
                    Destroy(gameObject);
                    // needs to invoke onDelete on Arrow of previous action
                } 
                return;
        }
    }
}
