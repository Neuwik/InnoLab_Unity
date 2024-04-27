using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateArrow : MonoBehaviour, IPointerClickHandler
{
    public GameObject Actionbox;
    public GameObject Arrow;

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (Actionbox.transform.parent.name == "UMLPanel")
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left: // Attach Arrow
                    if (GameManager.Instance.ActiveArrow != null &&
                        gameObject.CompareTag("UMLElement"))
                    {
                        GameManager.Instance.ActiveArrow.GetComponent<DrawArrow>().TargetElem = gameObject;
                        GameManager.Instance.ActiveArrow = null;
                    }
                    return;

                case PointerEventData.InputButton.Right: // Create Arrow
                    if(GameManager.Instance.ActiveArrow == null)
                    {
                        var newArrow = GameObject.Instantiate(Arrow, gameObject.transform);
                        newArrow.transform.SetAsFirstSibling();
                        var _ = gameObject.GetComponent<RectTransform>().rect;
                        newArrow.GetComponent<DrawArrow>().Startpos = (Vector2)gameObject.transform.position + new Vector2(_.width / 2, _.height / 2);
                        GameManager.Instance.ActiveArrow = newArrow;
                    }
                    return;

                case PointerEventData.InputButton.Middle: // Delete Arrow
                    Destroy(GameManager.Instance.ActiveArrow);
                    return;
            }
        } 
            
    }
}
