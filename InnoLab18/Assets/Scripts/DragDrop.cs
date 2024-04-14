using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform canvasRectT;

    private RectTransform rectT;
    private GameObject umlPanel;
    private RectTransform umlRectT;
    private GameObject selectionPanel;

    private void Start()
    {
        canvasRectT = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        selectionPanel = GameObject.FindGameObjectWithTag("SelectionPanel"); 
        umlPanel = GameObject.FindGameObjectWithTag("UMLPanel");
        umlRectT = umlPanel.GetComponent<RectTransform>();
        rectT = GetComponent<RectTransform>();
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
            newUMLElement.transform.SetParent(selectionPanel.transform);
            gameObject.transform.SetParent(umlPanel.transform);
        }
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectT.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.transform.position.y <= (canvasRectT.rect.height - umlRectT.rect.height) || // bottom bordercheck
            gameObject.transform.position.x <= (canvasRectT.rect.width - umlRectT.rect.width)  || // left bordercheck
            canvasRectT.rect.height <= (gameObject.transform.position.y + rectT.rect.height)  || // top bordercheck
            canvasRectT.rect.width <= (gameObject.transform.position.x + rectT.rect.width    )) // right bordercheck
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
