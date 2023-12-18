using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;

    public Vector3 mouseTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mouseTarget = raycastHit.point;
            //Debug.Log("Direction X: " + mouseTarget.x);
            //Debug.Log("Direction Z: " + mouseTarget.z);
        }
    }
}
