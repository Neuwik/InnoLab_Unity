using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UMLCreateTree : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputTreeName;

    public void OnCreateClick()
    {
        if (string.IsNullOrEmpty(inputTreeName.text))
        {
            UMLManager.Instance.CreateNewTree();
        }
        else
        {
            UMLManager.Instance.CreateNewTree(inputTreeName.text);
        }
    }
}
