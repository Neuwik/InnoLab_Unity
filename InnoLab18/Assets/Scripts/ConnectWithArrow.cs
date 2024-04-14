using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectWithArrow : MonoBehaviour, IPointerClickHandler
{
    public GameObject Arrow;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Instantiate(Arrow, gameObject.transform).GetComponent("");
    }
}
