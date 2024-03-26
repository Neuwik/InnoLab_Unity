using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIEventManager : MonoBehaviour
{
    public Button btn_CollectGarbage;
    public GameObject SelectedUMLElement { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        btn_CollectGarbage.onClick.AddListener(GameManager.Instance.PlayerGarbageCollector.CollectGarbage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAddComponent(InputValue value)
    {
        if (SelectedUMLElement == null)
        {
            Debug.LogError("No UML Element selected");
        }
        else
        {
            //Transform UMLPanel_Transform = GameManager.Instance.UMLPanel.transform;
            //GameObject newUMLElement = Instantiate(SelectedUMLElement, UMLPanel_Transform.position, Quaternion.identity, UMLPanel_Transform);


            RectTransform UMLPanel_Transform = GameManager.Instance.UMLPanel.GetComponent<RectTransform>();
            //RectTransform SelectedUMLElement_Transform = SelectedUMLElement.GetComponent<RectTransform>();
            float UMLPanel_Width = UMLPanel_Transform.rect.width;
            //float SelectedUMLElement_Width = SelectedUMLElement_Transform.rect.width;

            Vector3 position = UMLPanel_Transform.position;
            position.x -= (UMLPanel_Width / 2);
            GameObject newUMLElement = Instantiate(SelectedUMLElement, position, Quaternion.identity, UMLPanel_Transform);
        }
    }
}
