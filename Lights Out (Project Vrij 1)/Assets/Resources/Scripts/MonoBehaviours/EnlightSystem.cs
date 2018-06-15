using UnityEngine;
using System.Collections.Generic;

public class EnlightSystem : MonoBehaviour {

    public float enlighted = 0f;

    private List<Light> lights = new List<Light>();

    private void Update()
    {
        QueryLights();
        float intensity = 0;

        //Get the intensity factored by the light range for each light in range of the player
        for (int i = 0; i < lights.ToArray().Length; i++)
        {
            float dist = Vector3.Distance(lights[i].transform.position, transform.position);
            //Check if the player is close enough to the light:
            if (dist < lights[i].range / 4)
            {
                Debug.DrawLine(lights[i].transform.position, transform.position, Color.yellow);

                //Get the relative distance of the player to the light taking its intensity in account as well
                float rDist = (lights[i].range - dist);
                float distFromLight = rDist + (lights[i].intensity * rDist);

                intensity += Mathf.Log10(distFromLight);
            }
        }
        enlighted = intensity;
    }

    private void QueryLights()
    {
        Light[] l = FindObjectsOfType<Light>();
        lights.Clear();
        for (int i = 0; i < l.Length; i++)
        {
            lights.Add(l[i]);
        }
    }
}