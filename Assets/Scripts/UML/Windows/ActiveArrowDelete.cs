using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveArrowDelete : MonoBehaviour
{
    public UnityEvent OnArrowDelete = new UnityEvent();
    public void DeactivateButton()
    {
        gameObject.SetActive(false);
    }
    public void DeleteActiveArrow()
    {
        var CA = GameManager.Instance.ActiveArrow.transform.parent.GetComponent<CreateArrow>();
        CA.DeleteArrow();
        CA.setUMLActivity(false);
        DeactivateButton();
        OnArrowDelete.RemoveListener(DeactivateButton);
    }
}
