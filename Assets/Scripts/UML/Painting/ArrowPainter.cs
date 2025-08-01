using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArrowPainter : MonoBehaviour
{
    private float mouseOffset = 5f; //  3px does not work, it autosnaps the mouse click???

    private float conditionOffset = 10f;

    [SerializeField]
    private float minHeight = 10f;

    private bool _conditionOutcome;
    public bool ConditionOutcome { get { return _conditionOutcome; } set { _conditionOutcome = value; } }

    private CreateArrow _parentElem;
    private RectTransform _parentRect;

    private CreateArrow _targetElem;
    public CreateArrow GetTargetElm { get { return _targetElem; } }
    private RectTransform _targetRect;

    private AUMLElement _prev;

    private RectTransform _rect;
    [SerializeField]
    private RectTransform _visualRect;
    [SerializeField]
    private RectTransform _trueVisual;
    [SerializeField]
    private RectTransform _falseVisual;

    public UnityEvent<ArrowPainter> OnDelete;
    public GameObject DeleteButton;

    private Color _baseColor = Color.gray;
    private static Color _trueColor = Color.green * 0.8f;
    private static Color _falseColor = Color.red * 0.9f;

    [SerializeField]
    private TMP_Text _textField;
    private bool _isConditional = false;
    private bool _condition = true;
    private string _conditionalTrueText = "";
    private string _conditionalFalseText = "";
    private string _conditionTypeTag = "Normal";
    Vector2 _textsizeOffset = new Vector2(15, 10);


    public bool TrySetTargetElem(CreateArrow value)
    {
        if (_parentElem != null)
        {
            _parentElem.GetComponent<UMLHighlighter>().HighlightSwitch();
        }
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
        _prev.ChangeNextElement(_targetElem.GetComponent<AUMLElement>(), Condition);
        FindAndSetDeleteButton();

        return true;
    }
    public bool Condition
    {
        get { return _condition; }
        set
        {
            _isConditional = true;
            _condition = value;
            _visualRect.gameObject.SetActive(false);
            if (_condition) SetTrueArrow();
            else            SetFalseArrow();
            //_textField.gameObject.SetActive(_isConditional);
        }
    }

    public void ToggleCondition()
    {
        if (_isConditional)
        {
            if (_condition)
            {
                SetFalseArrow();
            }
            else
            {
                SetTrueArrow();
            }
        }
    }
    private void logRect(RectTransform rect)
    {
        Debug.Log($"{rect.name} \tPosition: {rect.position};\t LocalPosition: {rect.localPosition};");
    }
    private void SetTrueArrow()
    {
        _condition = true;

        //logRect(_visualRect);
        //logRect(_trueVisual);
        //logRect(_falseVisual);
        //_trueVisual.localPosition = _visualRect.localPosition;
        _visualRect = _trueVisual;
        _textField = _trueVisual.GetComponentInChildren<TextMeshProUGUI>();
        _textField.text = _conditionalTrueText;
        ChangeArrowColor(_trueColor);
        SetTrueArrowVisable();
        FindAndSetDeleteButton();
    }
    private void SetFalseArrow()
    {
        _condition = false;

        //logRect(_visualRect);
        //logRect(_trueVisual);
        //logRect(_falseVisual);
        //_falseVisual.GetComponentInChildren<Te> = _visualRect.localPosition;
        _visualRect = _falseVisual;
        _textField = _falseVisual.GetComponentInChildren<TextMeshProUGUI>();
        _textField.text = _conditionalFalseText;
        ChangeArrowColor(_falseColor);
        SetFalseArrowVisable();
        FindAndSetDeleteButton();
    }
    private void FindAndSetDeleteButton() 
    {
        DeleteButton = _visualRect.Find("UpperHorizontalShaft").GetComponentInChildren<Button>(true).gameObject;
    }
    private void SetFalseArrowVisable()
    {
        _visualRect.gameObject.SetActive(false);
        _trueVisual.gameObject.SetActive(false);
        _falseVisual.gameObject.SetActive(true);
    }
    private void SetTrueArrowVisable()
    {
        _visualRect.gameObject.SetActive(false);
        _falseVisual.gameObject.SetActive(false);
        _trueVisual.gameObject.SetActive(true);
    }

    private void TargetDestroyed()
    {
        Destroy(gameObject);
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

        if (_isConditional)
        { 
            if (gameObject.transform.parent.name == "ForLoop")
            {
                _conditionalFalseText = "afterwards";
                _conditionalTrueText = "do while";
            } else {
                _conditionalFalseText = "false";
                _conditionalTrueText = "true";
            }

            _textField.text = _condition ? _conditionalTrueText : _conditionalFalseText;
        }
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
        bool arrowGetsDragged = _targetElem == null;

        if (!arrowGetsDragged)
        {
            targetPos = _targetRect.position;
            targetSize = _targetRect.sizeDelta * _targetRect.lossyScale;
        }
        else
        {
            targetPos = (Vector2)Input.mousePosition;
            //targetPos = (Vector2)Input.mousePosition + mouseOffset; // causes bug when drawing arrow to the upper right
            //Debug.Log("MOUSE: " + targetPos);
        }

        Vector2 parentPos = _parentRect.position;
        Vector2 parentSize = _parentRect.sizeDelta * _parentRect.lossyScale;
        Vector2 conditionalTextOffset = new Vector2(0, 0) ;

        Vector2 direction = targetPos - parentPos;
        

        //Debug.Log(parentPos + " -> " + targetPos + " / " + direction);

        if (direction.y < 0 && direction.y * -1 > targetSize.y / 2 + parentSize.y / 2 + minHeight) // target is under parent
        {
            if (arrowGetsDragged)
            {
                targetPos.y += mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the left
                {
                    conditionalTextOffset = new Vector2(-(conditionOffset + _textsizeOffset.x), -_textsizeOffset.y);
                    parentPos.x -= conditionOffset;
                    targetPos.x -= conditionOffset;
                }
                else // move "false" arrow to the right
                {
                    
                    conditionalTextOffset = new Vector2(conditionOffset + _textsizeOffset.x, -_textsizeOffset.y);
                    parentPos.x += conditionOffset;
                    targetPos.x += conditionOffset;
                }
            }

            DrawArrow(
                parentPos - new Vector2(0, parentSize.y / 2),
                targetPos + new Vector2(0, targetSize.y / 2),
                conditionalTextOffset,
                180
            );
        }
        else if (direction.y > 0 && direction.y > targetSize.y / 2 + parentSize.y / 2 + minHeight) // taget is above parent
        {
            if (arrowGetsDragged)
            {
                targetPos.y -= mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the left
                {
                    conditionalTextOffset = new Vector2(-(conditionOffset + _textsizeOffset.x), _textsizeOffset.y);
                    parentPos.x -= conditionOffset;
                    targetPos.x -= conditionOffset;
                }
                else // move "false" arrow to the right
                {
                    conditionalTextOffset  = new Vector2(conditionOffset + _textsizeOffset.x, _textsizeOffset.y);
                    parentPos.x += conditionOffset;
                    targetPos.x += conditionOffset;
                }
            }

            DrawArrow(
                parentPos + new Vector2(0, parentSize.y / 2),
                targetPos - new Vector2(0, targetSize.y / 2),
                conditionalTextOffset,
                0
            );
        }
        else if (direction.x < 0 && direction.x * -1 > targetSize.x / 2 + parentSize.x / 2 + minHeight) // taget is left of parent
        {
            if (arrowGetsDragged)
            {
                targetPos.x += mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the up
                {
                    conditionalTextOffset = new Vector2(-_textsizeOffset.x, conditionOffset + _textsizeOffset.y);
                    parentPos.y += conditionOffset;
                    targetPos.y += conditionOffset;
                }
                else // move "false" arrow to the down
                {
                    conditionalTextOffset = new Vector2(-_textsizeOffset.x, -(conditionOffset + _textsizeOffset.y));
                    parentPos.y -= conditionOffset;
                    targetPos.y -= conditionOffset;
                }
            }

            DrawArrow(
                parentPos - new Vector2(parentSize.x / 2, 0),
                targetPos + new Vector2(targetSize.x / 2, 0),
                conditionalTextOffset,
                90
            );
        }
        else if (direction.x > 0 && direction.x > targetSize.x / 2 + parentSize.x / 2 + minHeight) // target is right of parent
        {
            if (arrowGetsDragged)
            {
                targetPos.x -= mouseOffset; // apply mouse offset, so that arrow won't be clicked when drawing
            }
            else if (_isConditional)
            {
                if (_condition) // move "true" arrow to the up
                {
                    conditionalTextOffset = new Vector2(_textsizeOffset.x, conditionOffset + _textsizeOffset.y);
                    parentPos.y += conditionOffset;
                    targetPos.y += conditionOffset;
                }
                else // move "false" arrow to the down
                {
                    conditionalTextOffset = new Vector2(_textsizeOffset.x, -(conditionOffset + _textsizeOffset.y));
                    parentPos.y -= conditionOffset;
                    targetPos.y -= conditionOffset;
                }
            }
            DrawArrow(
                parentPos + new Vector2(parentSize.x / 2, 0),
                targetPos - new Vector2(targetSize.x / 2, 0),
                conditionalTextOffset,
                -90
            );
        }
        else // target is inside of parent
        {
            _visualRect.gameObject.SetActive(false);
            //Debug.LogWarning("Can't redraw arrow");
        }
    }

    private void DrawArrow(Vector2 startPoint, Vector2 endPoint, Vector2 conditionalTextOffset, float rotation = 0)
    {
        _visualRect.gameObject.SetActive(true);
        _rect.position = startPoint;
        _rect.localRotation = Quaternion.Euler(0, 0, rotation);
        _textField.gameObject.transform.localRotation = Quaternion.Euler(0, 0, rotation * -1);
        _textField.gameObject.GetComponent<RectTransform>().position = startPoint + conditionalTextOffset;

        Vector2 size = endPoint - startPoint;
        size = _rect.InverseTransformVector(size);

        _rect.sizeDelta = size;
    }

    private void ChangeArrowColor(Color color)
    {
 
        foreach (var arrowPart in _visualRect.gameObject.GetComponentsInChildren<Image>(true))
        {
            arrowPart.color = color;
            _textField.color = color;
        }
    }
}