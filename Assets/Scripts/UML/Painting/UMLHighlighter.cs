using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UMLHighlighter : MonoBehaviour
{
    [SerializeField]
    private GameObject _highlightBackground;

    private bool _InHighlightMode = false;
    private void SetHighlightMode(bool value)
    {
        var CA = GetComponent<CreateArrow>();
        _InHighlightMode = value;
        _highlightBackground.SetActive(_InHighlightMode);
        if (GameManager.Instance.ActiveArrow == null && CA.Arrows.Count == CA.GetMaxArrowCount())
        {
            foreach (ArrowPainter arrow in CA.Arrows)
            {
                Debug.Log(arrow.DeleteButton.name + " = " + value);
                arrow.DeleteButton.SetActive(value);
            }
        }
    }
    public bool GetHighlightMode()
    {
        return _highlightBackground;
    }
    public void StartHighlightMode()
    {
        SetHighlightMode(true);
    }
    public void EndHighlightMode()
    {
        SetHighlightMode(false);
    }
    public void HighlightSwitch()
    {
        SetHighlightMode(!_InHighlightMode);
    }
    

}
