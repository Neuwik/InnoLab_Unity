using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public UnityEvent OnPossitionChanged;
    public UnityEvent OnDelete;

    private void Start()
    {
        _canvasRectT = GameManager.Instance.UML_Canvas.GetComponent<RectTransform>();
        _selectionPanel = GameManager.Instance.UML_SelectionPanel; 
        _umlPanel = GameManager.Instance.UML_Panel;
        _umlRectT = _umlPanel.GetComponent<RectTransform>();
        _rectT = GetComponent<RectTransform>();
    }

    private void Awake()
    {

    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Place new Object
        if (gameObject.transform.parent.CompareTag(_selectionPanel.tag))
        {
            var newUMLElement = Instantiate(
                gameObject,
                new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z),
                Quaternion.identity
            );
            newUMLElement.transform.SetParent(_selectionPanel.transform);
            gameObject.transform.SetParent(_umlPanel.transform);
        }
        GameManager.Instance.ReDrawArrow = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectT.anchoredPosition += eventData.delta;
        OnPossitionChanged.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.ReDrawArrow = false;
        if (gameObject.transform.position.y <= (_canvasRectT.rect.height - _umlRectT.rect.height) || // bottom bordercheck
            gameObject.transform.position.x <= (_canvasRectT.rect.width - _umlRectT.rect.width)  || // left bordercheck
            _canvasRectT.rect.height <= (gameObject.transform.position.y + _rectT.rect.height)  || // top bordercheck
            _canvasRectT.rect.width <= (gameObject.transform.position.x + _rectT.rect.width    )) // right bordercheck
        {
            gameObject.GetComponent<CreateArrow>().ReduceTargetAmount();
            OnDelete.Invoke();
            Destroy(gameObject);
        }
        Debug.Log("OnEndDrag");
    }
}
