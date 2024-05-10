using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    private int _targetMaxAmount;
    public int TargetAmount;

    public GameObject Actionbox;
    public GameObject Arrow;

    private void Start()
    {
        // a better solution would be to get the type from the original prefab, but I could get it to work
        Debug.Log(gameObject.name);
        if(gameObject.name.Contains("Is"))
        {
            _targetMaxAmount = 2;
        }
        else
        {
            _targetMaxAmount = 1;
        }
        TargetAmount = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Actionbox.transform.parent.name == "UMLPanel")
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left: // Attach Arrow happens on TargetObject
                    if (GameManager.Instance.ActiveArrow != null &&
                        gameObject.CompareTag("UMLElement"))
                    {
                        //++GameManager.Instance.ActiveArrow.transform.parent.GetComponent<CreateArrow>().TargetAmount;
                        GameManager.Instance.ActiveArrow.GetComponent<ArrowPainter>().TargetElem = gameObject;
                        GameManager.Instance.ActiveArrow = null;
                        
                    }
                    return;

                case PointerEventData.InputButton.Right: // Create Arrow happens on Parent Object
                    Debug.Log($"_targetAmount = {TargetAmount} / _targetMaxAmount = {_targetMaxAmount}");
                    if (GameManager.Instance.ActiveArrow == null &&
                        TargetAmount < _targetMaxAmount
                       )
                    {
                        ++TargetAmount;
                        var newArrow = GameObject.Instantiate(Arrow, gameObject.transform);
                        newArrow.transform.SetAsFirstSibling();
                        var _ = gameObject.GetComponent<RectTransform>().rect;
                        newArrow.GetComponent<ArrowPainter>().Startpos = (Vector2)gameObject.transform.position + new Vector2(_.width / 2, _.height / 2);
                        GameManager.Instance.ActiveArrow = newArrow;
                    }
                    return;

                case PointerEventData.InputButton.Middle: // Delete Arrow
                    ReduceTargetAmount();
                    Destroy(GameManager.Instance.ActiveArrow);
                    return;
            }
        } 
            
    }
    public void ReduceTargetAmount()
    {
        if (TargetAmount > 0)
        {
            --TargetAmount;
        }
    }
}
