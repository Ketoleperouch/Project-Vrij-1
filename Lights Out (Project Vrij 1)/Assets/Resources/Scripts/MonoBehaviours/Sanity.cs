using UnityEngine;
using UnityEngine.PostProcessing;

public class Sanity : MonoBehaviour
{
    public PostProcessingProfile profile;
    public float startIntensity = 0f;
    public float newIntensity = 1f;
    public bool insane;

    private float timer;
    private EnlightSystem enlightSystem;

    static float t = 0.0f;

    void Start()
    {
        enlightSystem = GetComponent<EnlightSystem>();
        insane = false;
        profile.vignette.enabled = true;
        Begin();
    }

    void Update()
    {
        if (!insane)
        {
            timer = Time.time + 4;
        }

        insane = enlightSystem.enlighted < 0.2f;

        if (Time.time > timer)
        {
            if (insane == true && t <= 1f)
            {
                t += 0.5f * Time.deltaTime;

                VignetteModel.Settings vignetteSettings = profile.vignette.settings;
                vignetteSettings.intensity = Mathf.Lerp(0.0f, 1.0f, t);
                profile.vignette.settings = vignetteSettings;
            }
        }
        if (insane == false && t >= 0f)
        {
            t -= 0.5f * Time.deltaTime;
            VignetteModel.Settings vignetteSettings = profile.vignette.settings;
            vignetteSettings.intensity = Mathf.Lerp(0f, 1f, t);
            profile.vignette.settings = vignetteSettings;
        }
    }

    void Begin()
    {
        VignetteModel.Settings vignetteSettings = profile.vignette.settings;
        vignetteSettings.intensity = startIntensity;
        profile.vignette.settings = vignetteSettings;
    }
}