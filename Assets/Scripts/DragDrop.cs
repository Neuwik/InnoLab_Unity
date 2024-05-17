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
        //Debug.Log("OnBeginDrag");

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
        if (Mathf.Abs(gameObject.transform.localPosition.y) + (_rectT.rect.height / 2) >= _umlRectT.rect.height / 2 || // top, bottom bordercheck
            Mathf.Abs(gameObject.transform.localPosition.x) + (_rectT.rect.width / 2) >= _umlRectT.rect.width / 2 )  // right, left bordercheck
        {
            gameObject.GetComponent<CreateArrow>().ReduceTargetAmount();
            OnDelete.Invoke();
            Destroy(gameObject);
        }
        //Debug.Log("OnEndDrag");
    }
}
