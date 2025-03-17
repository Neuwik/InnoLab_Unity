using UnityEngine;

public class UMLDelete : MonoBehaviour
{
    public long TreeID;

    public void DeleteCurrentTree()
    {
        if (!UMLManager.Instance.DeleteCurrentTree())
        {
            Debug.LogError("Could not delete current Tree");
        }
    }

    public void DeleteTree()
    {
        if (!UMLManager.Instance.DeleteTree(TreeID))
        {
            Debug.LogError("Could not delete Tree " + TreeID);
        }
    }
}
