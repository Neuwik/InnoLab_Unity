using UnityEngine;

public class UMLCallStack : MonoBehaviour
{
    public UMLCallStackElement CallStackElementPrefab;

    [SerializeField]
    private GameObject stack;

    public void AddCallStackElement(AUMLElement element)
    {
        UMLCallStackElement csElement = Instantiate(CallStackElementPrefab, stack.transform);
        csElement.UpdateTextAndBackground(element);
    }

    public void ClearCallStack()
    {
        foreach (Transform child in stack.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
