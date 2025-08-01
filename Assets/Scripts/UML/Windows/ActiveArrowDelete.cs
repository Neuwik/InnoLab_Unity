using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveArrowDelete : MonoBehaviour
{
    public UnityEvent OnArrowDelete = new UnityEvent();
    public void ButtonActivitySwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void DeleteActiveArrow()
    {
        var umlElem = GameManager.Instance.ActiveArrow.transform.parent;
        umlElem.GetComponent<CreateArrow>().DeleteArrow();
        umlElem.GetComponent<UMLHighlighter>().EndHighlightMode();
        gameObject.SetActive(false);
        OnArrowDelete.RemoveListener(ButtonActivitySwitch);
    }
}
