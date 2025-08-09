using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UMLHighlighter : MonoBehaviour
{
    [SerializeField]
    private GameObject _highlightBackground;
    [SerializeField]
    private GameObject _destroyButton;
    [SerializeField]
    private GameObject _switchButton;

    private bool _InHighlightMode = false;
    private CreateArrow _CA;
    void Start()
    {
        _CA = GetComponent<CreateArrow>(); 
    }
    private void SetHighlightMode(bool value)
    {
        _InHighlightMode = value;
        _highlightBackground.SetActive(_InHighlightMode);
        _destroyButton.SetActive(_InHighlightMode);
        
        if (GameManager.Instance.ActiveArrow == null && _CA.Arrows.Count == _CA.GetMaxArrowCount())
        {
            foreach (ArrowPainter arrow in _CA.Arrows)
            {
                arrow.DeleteButton.SetActive(value);
            }
            if (GameManager.Instance.UseBtnActions && _CA.Arrows.Count == 2)
            {
                _switchButton.SetActive(_InHighlightMode);
            }
        }
    }
    public bool GetHighlightMode()
    {
        return _InHighlightMode;
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
