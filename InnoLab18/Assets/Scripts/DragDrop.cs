using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _canvasRectT;
    private RectTransform _rectT;

    private GameObject _umlPanel;
    private RectTransform _umlRectT;
    private GameObject _selectionPanel;

    public UnityEvent OnPossitionChanged;

    private void Start()
    {
        _canvasRectT = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        _selectionPanel = GameObject.FindGameObjectWithTag("SelectionPanel"); 
        _umlPanel = GameObject.FindGameObjectWithTag("UMLPanel");
        _umlRectT = _umlPanel.GetComponent<RectTransform>();
        _rectT = GetComponent<RectTransform>();
    }

    private void Awake()
    {

    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Place new Object
        if (gameObject.transform.parent.CompareTag("SelectionPanel"))
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
        realignArrow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectT.anchoredPosition += eventData.delta;
        OnPossitionChanged.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.transform.position.y <= (_canvasRectT.rect.height - _umlRectT.rect.height) || // bottom bordercheck
            gameObject.transform.position.x <= (_canvasRectT.rect.width - _umlRectT.rect.width)  || // left bordercheck
            _canvasRectT.rect.height <= (gameObject.transform.position.y + _rectT.rect.height)  || // top bordercheck
            _canvasRectT.rect.width <= (gameObject.transform.position.x + _rectT.rect.width    )) // right bordercheck
        {
            Destroy(gameObject);
        }

        realignArrow();
        //Debug.Log("OnEndDrag");
    }
    private void realignArrow()
    {
        Transform attachedArrow;
        if ((attachedArrow = gameObject.transform.Find("Arrow(Clone)")) != null)
        {
            GameManager.Instance.ReDrawArrow = !GameManager.Instance.ReDrawArrow;
            attachedArrow.GetComponent<DrawArrow>().enabled = !attachedArrow.GetComponent<DrawArrow>().enabled;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
}
