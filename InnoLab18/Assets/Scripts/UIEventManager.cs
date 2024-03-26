using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventManager : MonoBehaviour
{
    public Button btn_CollectGarbage;
    // Start is called before the first frame update
    void Start()
    {
        btn_CollectGarbage.onClick.AddListener(GameManager.Instance.PlayerGarbageCollector.CollectGarbage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
