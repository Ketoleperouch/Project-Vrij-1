using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Sanity : MonoBehaviour
{
    public PostProcessingProfile profile;
    public float startIntensity = 0f;
    public float newIntensity = 1f;
    public bool insane;
    public AudioSource insanitySound;
    public CanvasGroup fader;
    
    public bool dead { get; set; }

    private float timer;
    private float deathTimer;
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
        if (dead)
        {
            Dead();
            return;
        }
        if (!insane)
        {
            timer = Time.time + 4;
            deathTimer = Time.time + 24;
        }

        insane = enlightSystem.enlighted < 0.05f;
        insanitySound.volume = t;

        if (Time.time > timer)
        {
            if (insane && t <= 1f)
            {
                t += 0.5f * Time.deltaTime;
                VignetteModel.Settings vignetteSettings = profile.vignette.settings;
                vignetteSettings.intensity = Mathf.Lerp(0.0f, 1.0f, t);
                profile.vignette.settings = vignetteSettings;
            }
            if (Time.time > deathTimer && insane)
            {
                //GameOver
                dead = true;
            }
        }
        if (!insane && t >= 0f)
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

    void Dead()
    {
        Debug.Log("Game Over!");
    }

    IEnumerator Fade()
    {
        while (fader.alpha > 0)
        {
            fader.alpha = Mathf.MoveTowards(fader.alpha, 1, Time.deltaTime);
            yield return null;
        }
    }
}