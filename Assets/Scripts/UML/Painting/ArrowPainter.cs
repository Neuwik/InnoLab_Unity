using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArrowPainter : MonoBehaviour
{
    private float mouseOffset = 5f; //  3px does not work, it autosnaps the mouse click???

    private float conditionOffset = 10f;

    [SerializeField]
    private float minHeight = 10;

    private bool _conditionOutcome;
    public bool ConditionOutcome { get { return _conditionOutcome; } set { _conditionOutcome = value; } }

    private CreateArrow _parentElem;
    private RectTransform _parentRect;

    private CreateArrow _targetElem;
    public CreateArrow GetTargetElm { get { return _targetElem; } }
    private RectTransform _targetRect;

    private AUMLElement _prev;

    private CreateArrow _prevCreateArrow;

    private RectTransform _rect;
    [SerializeField]
    private RectTransform _visualRect;

    public UnityEvent<ArrowPainter> OnDelete;

    private Color _baseColor = Color.gray;
    private static Color _trueColor = Color.green * 0.8f;
    private static Color _falseColor = Color.red * 0.9f;

    public bool TrySetTargetElem(CreateArrow value)
    {
        if (_parentElem == value || _targetElem == value)
        {
            return false;
        }

        _targetElem = value;
        _targetRect = _targetElem.GetComponent<RectTransform>();

        _targetElem.GetComponent<DragDrop>()?.OnStartedMoving.AddListener(EnableDrawing);
        _targetElem.GetComponent<DragDrop>()?.OnStoppedMoving.AddListener(DisableDrawing);
        _targetElem.GetComponent<DragDrop>()?.OnDelete.AddListener(TargetDestroyed);

        _prev = transform.parent.GetComponent<AUMLElement>();
        CreateArrow CA = _targetElem.GetComponent<CreateArrow>();
        CA.OnDelete.AddListener(TargetDestroyed);

        _prevCreateArrow = _prev.GetComponent<CreateArrow>();

        _prev.ChangeNextElement(_targetElem.GetComponent<AUMLElement>(), Condition);

        return true;
    }

    [SerializeField]
    private TMP_Text _textField;
    private bool _isConditional = false;
    private bool _condition = true;
    public bool Condition
    {
        get { return _condition; }
        set
        {
            _isConditional = true;
            _textField.gameObject.SetActive(_isConditional);
            ChangeArrowColor(_trueColor);
            if (_condition != value)
            {
                ToggleCondition();
            }
        }
    }

    public void ToggleCondition()
    {
        if (_isConditional)
        {
            if (_condition)
            {
                _textField.text = "false";
                ChangeArrowColor(_falseColor);
                _condition = false;
            }
            else
            {
                _textField.text = "true";
                ChangeArrowColor(_trueColor);
                _condition = true;
            }
        }
    }

    private void TargetDestroyed()
    {
        Destroy(gameObject);
        OnDelete.Invoke(this);
    }

    private void Awake()
    {
        _baseColor = GetComponentInChildren<Image>().color;
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _parentElem = gameObject.transform.parent.GetComponent<CreateArrow>();
        _parentRect = _parentElem.GetComponent<RectTransform>();

        // ? because Start Point has no DragDrop
        _parentElem.GetComponent<DragDrop>()?.OnStartedMoving.AddListener(EnableDrawing);
        _parentElem.GetComponent<DragDrop>()?.OnStoppedMoving.AddListener(DisableDrawing);
    }

    void Update()
    {
        DrawArrow();
    }

    private void EnableDrawing()
    {
        enabled = true;
    }
    private void DisableDrawing()
    {
        enabled = false;
    }

    private void DrawArrow()
    {
        Vector2 targetSize = Vector2.zero;
        Vector2 targetPos = Vector2.zero;
        bool targetIsMouse = _targetElem == null;

        if (!targetIsMouse)
        {
            targetPos = _targetRect.position;
            targetSize = _targetRect.sizeDelta;
        }
        else
        {
            targetPos = (Vector2)Input.mousePosition;
            //targetPos = (Vector2)Input.mousePosition + mouseOffset; // causes bug when drawing arrow to the upper right
            //Debug.Log("MOUSE: " + targetPos);
        }

        Vector2 parentPos = _parentRect.position;
        Vector2 parentSize = _parentRect.sizeDelta;

        Vector2 direction = targetPos - parentPos;

        //Debug.Log(parentPos + " -> " + targetPos + " / " + direction);

        if (direction.y < 0 && direction.y * -1 > targetSize.y / 2 + parentSize.y / 2 + minHeight) // taget is under parent
        {
            if(targetIsMouse)
            {
                targetPos.y += mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if(_isConditional)
            {
                if(_condition) // move "true" arrow to the left
                {
                    parentPos.x -= conditionOffset;
                    targetPos.x -= conditionOffset;
                }
                else // move "false" arrow to the right
                {
                    parentPos.x += conditionOffset;
                    targetPos.x += conditionOffset;
                }
            }

            DrawArrow(
                parentPos - new Vector2(0, parentSize.y / 2),
                targetPos + new Vector2(0, targetSize.y / 2),
                180
            );
        }
        else if (direction.y > 0 && direction.y > targetSize.y / 2 + parentSize.y / 2 + minHeight) // taget is above parent
        {
            if (targetIsMouse)
            {
                targetPos.y -= mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the left
                {
                    parentPos.x -= conditionOffset;
                    targetPos.x -= conditionOffset;
                }
                else // move "false" arrow to the right
                {
                    parentPos.x += conditionOffset;
                    targetPos.x += conditionOffset;
                }
            }

            DrawArrow(
                parentPos + new Vector2(0, parentSize.y / 2),
                targetPos - new Vector2(0, targetSize.y / 2),
                0
            );
        }
        else if (direction.x < 0 && direction.x * -1 > targetSize.x / 2 + parentSize.x / 2 + minHeight) // taget is left of parent
        {
            if (targetIsMouse)
            {
                targetPos.x += mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the up
                {
                    parentPos.y += conditionOffset;
                    targetPos.y += conditionOffset;
                }
                else // move "false" arrow to the down
                {
                    parentPos.y -= conditionOffset;
                    targetPos.y -= conditionOffset;
                }
            }

            DrawArrow(
                parentPos - new Vector2(parentSize.x / 2, 0),
                targetPos + new Vector2(targetSize.x / 2, 0),
                90
            );
        }
        else if (direction.x > 0 && direction.x > targetSize.x / 2 + parentSize.x / 2 + minHeight) // taget is right of parent
        {
            if (targetIsMouse)
            {
                targetPos.x -= mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the up
                {
                    parentPos.y += conditionOffset;
                    targetPos.y += conditionOffset;
                }
                else // move "false" arrow to the down
                {
                    parentPos.y -= conditionOffset;
                    targetPos.y -= conditionOffset;
                }
            }

            DrawArrow(
                parentPos + new Vector2(parentSize.x / 2, 0),
                targetPos - new Vector2(targetSize.x / 2, 0),
                -90
            );
        }
        else // taget is inside of parent
        {
            _visualRect.gameObject.SetActive(false);
            //Debug.LogWarning("Can't redraw arrow");
        }
    }

    private void DrawArrow(Vector2 startPoint, Vector2 endPoint, float rotation = 0)
    {
        _visualRect.gameObject.SetActive(true);
        _rect.position = startPoint;
        _rect.localRotation = Quaternion.Euler(0, 0, rotation);
        _textField.gameObject.transform.localRotation = Quaternion.Euler(0, 0, rotation * -1);

        Vector2 size = endPoint - startPoint;
        size = _rect.InverseTransformVector(size);

        _rect.sizeDelta = size;
    }

    private void ChangeArrowColor(Color color)
    {
        foreach (var arrowPart in GetComponentsInChildren<Image>())
        {
            arrowPart.color = color;
        }
    }
}