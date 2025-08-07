using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteArrow : MonoBehaviour
{
    CreateArrow _CreateArrow;
    void Start()
    {
        _CreateArrow = transform.parent.gameObject.GetComponent<CreateArrow>();
    }
    public void DeleteThisArrow()
    {
        _CreateArrow.DeleteArrow(transform.GetComponent<ArrowPainter>());
        _CreateArrow.GetComponent<UMLHighlighter>().EndHighlightMode();
    }
}
