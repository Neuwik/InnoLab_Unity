using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _canvasRectT;
    private RectTransform _rectT;

    private GameObject _umlPanel;
    private RectTransform _umlRectT;
    private GameObject _selectionPanel;

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
            // Set new Parents
            newUMLElement.transform.SetParent(_selectionPanel.transform);
            gameObject.transform.SetParent(_umlPanel.transform);
        }
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        _rectT.anchoredPosition += eventData.delta;
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
        //Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
}
