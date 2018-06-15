using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour {

    public UnityEvent onActivate;

    private bool active = false;
    private ParticleSystem particles;
#if UNITY_EDITOR
    private new MeshRenderer renderer;
#else
    private MeshRenderer renderer;
#endif

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (active)
        {
            renderer.material.SetColor("_EmissionColor", Color.white * Mathf.Lerp(renderer.material.GetColor("_EmissionColor").maxColorComponent, 2, Time.deltaTime * 0.1f));
        }
    }

    public void Activate()
    {
        if (particles)
            particles.Play();
        onActivate.Invoke();
        active = true;
    }

}