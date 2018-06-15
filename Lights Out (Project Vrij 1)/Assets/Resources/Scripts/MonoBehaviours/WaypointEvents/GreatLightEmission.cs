using System.Collections;
using UnityEngine;

public class GreatLightEmission : WaypointEvent {

    public float lightRange = 40;
    public float lightIntensity = 1;
    public float transitionSpeed = 5;

	public override void Execute(FlameBehaviour flame)
    {
        StartCoroutine(Enlight(flame.flameLight));
    }

    private IEnumerator Enlight(Light flame)
    {
        while (!Mathf.Approximately(flame.intensity, lightIntensity) && !Mathf.Approximately(flame.range, lightRange))
        {
            flame.intensity = Mathf.Lerp(flame.intensity, lightIntensity, Time.deltaTime * transitionSpeed);
            flame.range = Mathf.Lerp(flame.range, lightRange, Time.deltaTime * transitionSpeed);
            yield return null;
        }
    }
}
