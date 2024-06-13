using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    public int TargetMaxAmount;
    public int TargetAmount;

    public GameObject Actionbox;
    public GameObject Arrow;

    public UnityEvent OnDelete;

    private void Start()
    {
        // a better solution would be to get the type from the original prefab, but I could get it to work
        //Debug.Log(gameObject.name);
        if(gameObject.name.Contains("Is"))
        {
            TargetMaxAmount = 2;
        }
        else
        {
            TargetMaxAmount = 1;
        }
        TargetAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Actionbox.transform.parent.name != "UMLPanel")
        {
            return;
        }
        GameObject childHelper;
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            childHelper = gameObject.transform.GetChild(0).gameObject;
            if (childHelper.name.Contains("Arrow"))
            {
                ReduceTargetAmount();
                Destroy(childHelper);
            }
            else
            {
                Destroy(gameObject);
            }
            return;
        }
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left: // Attach Arrow -> happens on TargetObject
                if (GameManager.Instance.ActiveArrow != null &&
                    gameObject.CompareTag("UMLElement"))
                {
                    GameManager.Instance.ActiveArrow.GetComponent<ArrowPainter>().TargetElem = gameObject;
                    GameManager.Instance.ActiveArrow = null;
                }
                return;

            case PointerEventData.InputButton.Right: 

                // change true or false if arrows starts in conditionblock
                if (TargetAmount == 2)
                {
                    foreach (ArrowPainter arrow in gameObject.GetComponentsInChildren<ArrowPainter>())
                    {
                        TMPro.TextMeshProUGUI conditionText = arrow.GetComponentInChildren<TMPro.TextMeshProUGUI>();

                        if (conditionText.text.Contains("true"))
                        {
                            conditionText.text = "false";
                        }
                        else
                        {
                            conditionText.text = "true";
                        }
                    }
                    /*for (int i = 0; i < 2; i++)
                    {
                        childHelper = gameObject.transform.GetChild(i).gameObject.transform.GetChild(5).gameObject;

                        TMPro.TextMeshProUGUI conditionText = childHelper.GetComponent<TMPro.TextMeshProUGUI>();

                        if (conditionText.text.Contains("true"))
                        {
                            conditionText.text = "false";
                        }
                        else
                        {
                            conditionText.text = "true";
                        }

                    }*/
                    gameObject.GetComponent<UMLCondition>().SwitchNextActions();

                    return;
                }

                // Create Arrow happens on Parent Object
                if (GameManager.Instance.ActiveArrow == null &&
                    TargetAmount < TargetMaxAmount )
                {
                    var newArrow = GameObject.Instantiate(Arrow, gameObject.transform);
                    newArrow.transform.SetAsFirstSibling();
                    Rect _ = gameObject.GetComponent<RectTransform>().rect;
                    newArrow.GetComponent<ArrowPainter>().StartPos = (Vector2)gameObject.transform.position + new Vector2(_.width / 2, _.height / 2);
                    IncreaseTargetAmount();
                    if (TargetMaxAmount == 2) // => only Condition blocks
                    {
                        childHelper = newArrow.transform.GetChild(5).gameObject;
                        Debug.Log(childHelper.name);
                        childHelper.SetActive(true);
                        if (TargetAmount == 2)
                        {
                            childHelper.GetComponent<TMPro.TextMeshProUGUI>().text = "false";
                        }
                    }
                    GameManager.Instance.ActiveArrow = newArrow;
                    
                }
                return;

            case PointerEventData.InputButton.Middle: // Delete Arrow
                GameObject child = gameObject.transform.GetChild(0).gameObject;
                if (child.name.Contains("Arrow"))
                {
                    ReduceTargetAmount();
                    Destroy(child);
                }
                else
                {
                    Destroy(gameObject);
                    OnDelete.Invoke();
                } 
                return;
        }
        
            
    }
    public void ReduceTargetAmount()
    {
        if (TargetAmount > 0)
        {
            --TargetAmount;
        }
    }
    public void IncreaseTargetAmount()
    {
        if (TargetAmount < TargetMaxAmount)
        {
            ++TargetAmount;
        }
    }
}
