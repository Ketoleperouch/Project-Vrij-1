using UnityEngine;

public class Torch : GameEvent
{
    public Light lamp;
    public float burningLength = 25;

    private bool burning = false;
    private float timer;

    public override void Execute()
    {
        lamp.enabled = true;
        burning = true;
        lamp.intensity = 1;
        timer = Time.time + burningLength;
    }

    private void Update()
    {
        if (burning && Time.time > timer)
        {
            lamp.intensity = Mathf.MoveTowards(lamp.intensity, 0, Time.deltaTime * 0.25f);
            if (lamp.intensity <= 0)
            {
                burning = false;
            }
        }
    }
}