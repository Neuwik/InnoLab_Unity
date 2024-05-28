using UnityEngine;
using System.Collections;

public class Big_Splash : MonoBehaviour
{
    public GameObject BigSplash;
    private float splashFlag = 0;

    void Start()
    {
        BigSplash.SetActive(false);
    }

    // Public method to trigger the splash effect
    public void TriggerSplashEffect()
    {
        if (splashFlag == 0)
        {
            StartCoroutine(ActivateAndTriggerSplash());
        }
    }

    private IEnumerator ActivateAndTriggerSplash()
    {
        BigSplash.SetActive(true);
        yield return null;
        StartCoroutine(TriggerSplash());
    }

    private IEnumerator TriggerSplash()
    {
        splashFlag = 1;
        yield return new WaitForSeconds(3.5f);
        BigSplash.SetActive(false);
        splashFlag = 0;
    }
}
