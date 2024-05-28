using UnityEngine;

public class VFXController : MonoBehaviour
{
    public ParticleSystem rocketTrail;
    public GameObject waterSplash;
    public Big_Splash waterSplashscript;

    public bool activateRocketTrail;

    private float splashFlag = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (activateRocketTrail)
        {
            if (!rocketTrail.isPlaying)
            {
                rocketTrail.Play();
            }
        }
        else
        {
            if (rocketTrail.isPlaying)
            {
                rocketTrail.Stop();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (waterSplashscript != null)
            {
                waterSplash.SetActive(true);
                waterSplashscript.TriggerSplashEffect();
            }
        }
    }
}
