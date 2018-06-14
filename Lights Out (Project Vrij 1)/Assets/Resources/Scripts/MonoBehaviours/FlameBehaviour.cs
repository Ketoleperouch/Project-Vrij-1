using UnityEngine;

public class FlameBehaviour : MonoBehaviour {

    [Range(0, 100)]
    public float vitality = 100f;
    public float diminishMultiplier = 5f;
    public float pulseRange = 5f;
    public float threshold = 40f;

    [ColorUsage(false)]
    public Color neutralColor = Color.white;
    [ColorUsage(false)]
    public Color lowEnergyColor = Color.white;

    public GameObject healParticles;
    public GameObject pulse;
    public AudioClip noPulseClip;

    private ParticleSystem particles;
    private float originalROT;
    private float originalLightIntensity;
    private Light flameLight;
    private AudioSource source;
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        particles = GetComponent<ParticleSystem>();
        flameLight = GetComponentInChildren<Light>(false);
        originalROT = particles.emission.rateOverTime.constant;
        originalLightIntensity = flameLight.intensity;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (vitality > 0)
        {
            vitality -= Time.deltaTime * diminishMultiplier;
        }
        vitality = Mathf.Clamp(vitality, 0, 100);


        if (Input.GetMouseButtonDown(1))
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
        if (vitality > threshold)
            startColor.startColor = (Color)Vector4.Lerp(startColor.startColor.color, new Color(neutralColor.r, neutralColor.g, neutralColor.b, vitality / 100), Time.deltaTime);
        else
            startColor.startColor = (Color)Vector4.Lerp(startColor.startColor.color, new Color(lowEnergyColor.r, lowEnergyColor.g, lowEnergyColor.b, vitality / 100), Time.deltaTime);

        flameLight.color = Vector4.Lerp(flameLight.color, startColor.startColor.color, Time.deltaTime);
    }

    private void SetEmission()
    {
        ParticleSystem.MinMaxCurve newRate = new ParticleSystem.MinMaxCurve{constant = originalROT * (vitality / 100)};

        var emission = particles.emission;
        emission.rateOverTime = newRate.constant;
    }

    private void Pulse()
    {
        //Pulse can only be activated when the vitality is sufficient. Otherwise, play a "cannot do this" sound.
        if (vitality > threshold && Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            Instantiate(pulse, transform.position, Quaternion.identity);
            vitality -= 10f;
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
        else if (Vector3.Distance(player.transform.position, transform.position) < 5f)
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
