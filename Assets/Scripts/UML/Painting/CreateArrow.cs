using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    private bool _ignoreNextArrowDrawInput = false;
    private int _maxArrowCount = 0;
    private GameObject _highlightBackground;
    private int getMaxArrowCount()
    {
        if (_maxArrowCount == 0)
        {
            _maxArrowCount = TryGetComponent<AUMLElementTrueFalse>(out _) ? 2 : 1;
        }
        return _maxArrowCount;
    }

    private List<ArrowPainter> _arrows = new List<ArrowPainter>();
    private void AddArrow(ArrowPainter arrow)
    {
        arrow.OnDelete.AddListener(RemoveArrow);
        _arrows.Add(arrow);
        if (arrow.GetTargetElm != null)
        {
            GetComponent<AUMLElement>().ChangeNextElement(arrow.GetTargetElm.GetComponent<AUMLElement>(), arrow.Condition);
        }
    }
    private void RemoveArrow(ArrowPainter arrow)
    {
        _arrows.Remove(arrow);
        arrow?.OnDelete.RemoveListener(RemoveArrow);
        GetComponent<AUMLElement>().ChangeNextElement(null, arrow.Condition);
    }

    public ArrowPainter ArrowPrefab;

    public UnityEvent OnDelete;
    public bool CanDraw = false;

    [SerializeField]
    private bool _isDeleteable = true;

    public CreateArrow initialConnection = null;
    public CreateArrow initialFalseConnection = null;

    private void Start()
    {
        if (initialConnection != null)
        {
            DrawArrowToElement(initialConnection);
        }
        if (initialFalseConnection != null && _maxArrowCount > 1)
        {
            DrawArrowToElement(initialFalseConnection, false);
        }
        //try {
        _highlightBackground = gameObject.transform.Find("HighlightImage").gameObject;
        //}
        //catch (NullReferenceException){
        //    _highlightBackground = null;
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanDraw || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if (_ignoreNextArrowDrawInput)
        {
            _ignoreNextArrowDrawInput = false;
            return;
        }

       
        if (GameManager.Instance.ActiveArrow == null && _arrows.Count < getMaxArrowCount())
        {
            UMLHighlightswitch();
            ArrowPainter newArrow = Instantiate(ArrowPrefab, gameObject.transform);
            newArrow.transform.SetAsFirstSibling();

            AddArrow(newArrow);

            if (getMaxArrowCount() == 2) // => only Condition blocks
            {
                if (_arrows.Count > 1) // second arrow
                {
                    newArrow.Condition = !(_arrows[0].Condition);
                }
                else // first arrow
                {
                    newArrow.Condition = true;
                }
            }
            GameManager.Instance.ActiveArrow = newArrow;
            return;
        }
        else if (GameManager.Instance.ActiveArrow != null)
        {
            if (GameManager.Instance.ActiveArrow.TrySetTargetElem(this))
            {
                GameManager.Instance.ActiveArrow = null;
                return;
            }
            GameManager.Instance.ActiveArrow.transform.parent.GetComponent<CreateArrow>().DeleteArrow();
            return;
        }
        else if (_arrows.Count == 2) // Switch True and False Arrow
        {
            foreach (ArrowPainter arrow in _arrows)
            {
                arrow.ToggleCondition();
            }

            gameObject.GetComponent<AUMLElementTrueFalse>().SwitchNextActions();

            return;
        }
        
        
        // below mousecontrol + inbetween code - Touchscreen code moved to above
        /*
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left: // Attach Arrow -> happens on TargetObject

                if (GameManager.Instance.ActiveArrow == null &&
                         _arrows.Count < getMaxArrowCount())
                {
                    UMLHighlightswitch();
                    ArrowPainter newArrow = Instantiate(ArrowPrefab, gameObject.transform);
                    newArrow.transform.SetAsFirstSibling();

                    AddArrow(newArrow);

                    if (getMaxArrowCount() == 2) // => only Condition blocks
                    {
                        if (_arrows.Count > 1) // second arrow
                        {
                            newArrow.Condition = !(_arrows[0].Condition);
                        }
                        else // first arrow
                        {
                            newArrow.Condition = true;
                        }
                    }
                    GameManager.Instance.ActiveArrow = newArrow;
                }
                else if (GameManager.Instance.ActiveArrow != null)
                {
                    if (GameManager.Instance.ActiveArrow.TrySetTargetElem(this))
                    {
                        GameManager.Instance.ActiveArrow = null;
                    }
                }
                else if (eventData.clickCount >= 2)
                {
                    DeleteArrow();
                }
                else if (_arrows.Count == 2) // Switch True and False Arrow
                {
                    foreach (ArrowPainter arrow in _arrows)
                    {
                        arrow.ToggleCondition();
                    }

                    gameObject.GetComponent<AUMLElementTrueFalse>().SwitchNextActions();

                    return;
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
                    _arrows.Count < getMaxArrowCount())
                {
                    ArrowPainter newArrow = Instantiate(ArrowPrefab, gameObject.transform);
                    newArrow.transform.SetAsFirstSibling();

                    AddArrow(newArrow);

                    if (getMaxArrowCount() == 2) // => only Condition blocks
                    {
                        if (_arrows.Count > 1) // second arrow
                        {
                            newArrow.Condition = !(_arrows[0].Condition);
                        }
                        else // first arrow
                        {
                            newArrow.Condition = true;
                        }
                    }
                    GameManager.Instance.ActiveArrow = newArrow;
                }
                return;
            

            case PointerEventData.InputButton.Middle: // Delete Block/Arrow
                
                DeleteArrow();
                return;
        }
        */
    }
    public void IgnoreNextArrowDrawInput()
    {
        _ignoreNextArrowDrawInput = true;
    }
    public void UMLHighlightswitch()
    {
        if (_highlightBackground != null) 
        {
            _highlightBackground.SetActive(!_highlightBackground.activeSelf);
        }
    }
    public void setUMLHighlightVisable(bool enabled)
    {
        _highlightBackground.SetActive(enabled);
    }

    public void DeleteArrow(ArrowPainter arrow = null) 
    {

        if (GameManager.Instance.UMLIsRunning) // disable deletion when UML is running
        {
            return;
        }

        if (arrow != null) 
        {
            RemoveArrow(arrow);
            Destroy(arrow.gameObject);
        }
        else if (_arrows.Count > 0)
        {
            arrow = _arrows.Last();
            RemoveArrow(arrow);
            Destroy(arrow.gameObject);
        }
        else
        {
            if (!_isDeleteable)
            {
                return;
            }

            OnDelete.Invoke();
            Destroy(gameObject);
            // needs to invoke onDelete on Arrow of previous action
        }
    }
    public bool DrawArrowToElement(CreateArrow target, bool condition = true)
    {
        if (target == null)
        {
            return false;
        }

        if (_arrows.Count < getMaxArrowCount())
        {
            ArrowPainter newArrow = Instantiate(ArrowPrefab, gameObject.transform);
            newArrow.transform.SetAsFirstSibling();

            if (getMaxArrowCount() == 2) // => only Condition blocks
            {
                newArrow.Condition = condition;
            }

            if (!newArrow.TrySetTargetElem(target))
            {
                Debug.LogWarning($"Create Arrow: Draw Arrow To Element failed because of Try Set Target Elem {target.name}");
                Destroy(newArrow.gameObject);
                return false;
            }

            AddArrow(newArrow);

            return true;
        }

        Debug.LogWarning($"Create Arrow: Draw Arrow To Element failed because of arrow counts {_arrows.Count} < {getMaxArrowCount()}");

        return false;
    }
}
