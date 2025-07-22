using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeleteArrow : MonoBehaviour
{
    private CreateArrow _arrowCreator;
    private void Start()
    {
        _arrowCreator = gameObject.transform.parent.GetComponent<CreateArrow>();
    }
    public void ArrowClick(PointerEventData eventData)
    { 
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _arrowCreator.DeleteArrow();
        }

    }
}
