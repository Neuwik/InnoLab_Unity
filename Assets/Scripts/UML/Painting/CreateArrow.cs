using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    private int _maxArrowCount = 0;
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
                else if (GameManager.Instance.ActiveArrow != null)
                {
                    if (GameManager.Instance.ActiveArrow.TrySetTargetElem(this))
                    {
                        GameManager.Instance.ActiveArrow = null;
                    }
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
            /*
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
            */

            case PointerEventData.InputButton.Middle: // Delete Block/Arrow

                if (GameManager.Instance.UMLIsRunning) // disable deletion when UML is running
                {
                    return;
                }

                if (_arrows.Count > 0)
                {
                    ArrowPainter arrow = _arrows.Last();
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
                return;
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
