using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private GameObject umlField;

    private void Start()
    {
        umlField = GameObject.FindGameObjectWithTag("UMLPanel");
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Place new Object
        GameObject selectionPanel = GameObject.Find("SelectionPanel");
        GridLayout grid = selectionPanel.GetComponent<GridLayout>();
        /*GameObject newElement = gameObject;

        newElement.transform.position = grid.WorldToCell(gameObject.transform.position);*/
        //grid.GetComponent<GridLayout>().Instantiate(newElement);
        var newElement = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        newElement.transform.parent = gameObject.transform;

        //remove old Object from GridLayout
        gameObject.transform.parent = umlField.transform;
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (umlField.GetComponent<RectTransform>().rect.Contains(rectTransform.anchoredPosition))
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
