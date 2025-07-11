using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _canvasRectT;
    private RectTransform _rectT;

    private GameObject _umlPanel;
    private RectTransform _umlRectT;
    private GameObject _selectionPanel;

    public UnityEvent OnPossitionChanged; // not used
    public UnityEvent OnStartedMoving;
    public UnityEvent OnStoppedMoving;
    public UnityEvent OnDelete;

    private void Start()
    {
        _canvasRectT = GameManager.Instance.UML_Canvas.GetComponent<RectTransform>();
        _selectionPanel = GameManager.Instance.UML_SelectionPanel;
        _umlPanel = GameManager.Instance.UMLWindow.BuildArea;
        _umlRectT = _umlPanel.GetComponent<RectTransform>();
        _rectT = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if the parent matches the selection panel
        if (gameObject.transform.parent.CompareTag(_selectionPanel.tag))
        {
            // Instantiate a clone of the current object
            var newUMLElement = Instantiate(gameObject);

            // Set the new object's parent to the selection panel
            newUMLElement.transform.SetParent(gameObject.transform.parent, true);

            // Copy local position and scale from the original object
            newUMLElement.transform.localScale = gameObject.transform.localScale;
            newUMLElement.transform.localPosition = gameObject.transform.localPosition;

            // Reassign the current object's parent to the UML Manager's current tree
            gameObject.transform.SetParent(UMLManager.Instance.CurrentTree.transform, true);

            // Enable arrow drawing on the current object
            //var arrowCreator = GetComponent<CreateArrow>();
            //if (arrowCreator != null)
            //{
            //    arrowCreator.CanDraw = true;
            //}
            //else
            //{
            //    Debug.LogWarning("CreateArrow component is missing on the GameObject.");
            //}
        }

        // Invoke drag started event, if assigned
        OnStartedMoving?.Invoke();
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.UMLIsRunning) // disable dragging when UML is running (tro prevent Block deletion)
        {
            return;
        }

        _rectT.anchoredPosition += (eventData.delta / _rectT.lossyScale);
        //OnPossitionChanged.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Vector3 posInUml = _umlRectT.InverseTransformPoint(transform.position);
        Vector3 posInUml = transform.localPosition;
        OnStoppedMoving.Invoke();
        /*
        if (Mathf.Abs(gameObject.transform.localPosition.y) + (_rectT.rect.height / 2) >= _umlRectT.rect.height / 2 || // top, bottom bordercheck
            Mathf.Abs(gameObject.transform.localPosition.x) + (_rectT.rect.width / 2) >= _umlRectT.rect.width / 2 )  // right, left bordercheck
        */
        // Enable arrow drawing on the current object
        var arrowCreator = GetComponent<CreateArrow>();
        if (arrowCreator != null)
        {
            arrowCreator.CanDraw = true;
        }
        else
        {
            Debug.LogWarning("CreateArrow component is missing on the GameObject.");
        }

        if (Mathf.Abs(posInUml.y) + (_rectT.rect.height / 2) >= _umlRectT.rect.height / 2 || // top, bottom bordercheck
            Mathf.Abs(posInUml.x) + (_rectT.rect.width / 2) >= _umlRectT.rect.width / 2)  // right, left bordercheck
        {
            //TODO???: gameObject.GetComponent<CreateArrow>().ReduceTargetAmount();
            //OnDelete.Invoke();
            //Destroy(gameObject);
            DestroyElement();
        }

        //Debug.Log("OnEndDrag");
    }
    public void DestroyElement()
    {
        OnDelete.Invoke();
        Destroy(gameObject);
    }
}
