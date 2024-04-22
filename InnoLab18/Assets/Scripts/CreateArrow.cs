using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    public DrawArrow Arrow;

    public void OnPointerClick(PointerEventData eventData)
    {
        var newArrow = GameObject.Instantiate(Arrow, gameObject.transform);
        newArrow.Startpos = gameObject.transform.position;
    }
}
