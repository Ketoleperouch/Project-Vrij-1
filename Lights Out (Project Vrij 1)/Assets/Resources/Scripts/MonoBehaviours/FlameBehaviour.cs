using UnityEngine;

public class FlameBehaviour : MonoBehaviour {

    [Range(0, 100)]
    public float vitality = 100f;
    public float diminishMultiplier = 5f;
    public float pulseRange = 5f;

    public GameObject pulse;
    public AudioClip noPulseClip; 

    private ParticleSystem particles;
    private float originalROT;
    private float originalLightIntensity;
    private Light flameLight;
    private AudioSource source;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        flameLight = GetComponentInChildren<Light>(false);
        originalROT = particles.emission.rateOverTime.constant;
        originalLightIntensity = flameLight.intensity;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        vitality -= Time.deltaTime * diminishMultiplier;

        if (Input.GetMouseButtonDown(0))
        {
            Pulse();
        }

        SetValues();
    }

    private void SetValues()
    {
        flameLight.intensity = originalLightIntensity * (vitality / 100);
        source.volume = vitality / 100;
        SetEmission();
        SetStartColor();
    }

    private void SetStartColor()
    {
        var startColor = particles.main;
        startColor.startColor = new Color(startColor.startColor.color.r, startColor.startColor.color.g, startColor.startColor.color.b, vitality / 100);
    }

    private void SetEmission()
    {
        ParticleSystem.MinMaxCurve newRate = new ParticleSystem.MinMaxCurve();
        newRate.constant = originalROT * (vitality / 100);

        var emission = particles.emission;
        emission.rateOverTime = newRate.constant;
    }

    private void Pulse()
    {
        //Pulse can only be activated when the vitality is sufficient. Otherwise, play a "cannot do this" sound.
        if (vitality > 30f)
        {
            Instantiate(pulse, transform.position, Quaternion.identity);
            vitality -= 20f;
            //Search for interactables and activate them:
            Collider[] pulseHits = Physics.OverlapSphere(transform.position, pulseRange);
            for (int i = 0; i < pulseHits.Length; i++)
            {
                InteractableBase interactable = pulseHits[i].GetComponent<InteractableBase>();

                if (interactable)
                {
                    interactable.Activate();
                }
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(noPulseClip, transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pulseRange);
    }
}
