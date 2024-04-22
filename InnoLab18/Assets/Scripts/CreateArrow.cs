using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    private GameObject _umlPanel;
    public DrawArrow Arrow;

    public void OnPointerClick(PointerEventData eventData)
    {
        _umlPanel = GameObject.FindGameObjectWithTag("UMLPanel"); ;
        var newArrow = GameObject.Instantiate(Arrow, gameObject.transform);
        newArrow.transform.SetParent(_umlPanel.transform);
        newArrow.Startpos = gameObject.transform.position;
    }
}
