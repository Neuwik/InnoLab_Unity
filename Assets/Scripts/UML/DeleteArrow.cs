using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteArrow : MonoBehaviour
{
    GameObject _arrow;
    public void Start()
    {
        _arrow = transform.parent.gameObject;
    }
    public void DeleteThisArrow() 
    {
        _arrow.GetComponent<CreateArrow>().DeleteArrow(transform.GetComponent<ArrowPainter>());
    }
}
