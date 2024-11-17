using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UMLCallStackElement : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textField;
    [SerializeField]
    private Image background;

    public void UpdateTextAndBackground(AUMLElement element)
    {
        textField.text = element.Name;
        background.sprite = element.Image.sprite;
        background.color = element.Image.color;
    }
}
