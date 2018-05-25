using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour {

    public UnityEvent onActivate;

    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void Activate()
    {
        particles.Play();
        onActivate.Invoke();
    }

}
