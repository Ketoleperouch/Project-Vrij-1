using System.Collections;
using UnityEngine;

public class WaypointSpeedChanger : MonoBehaviour {

    [Tooltip("The speed variable determines the speed of the energy source when it travels to this waypoint (so not when it has reached this waypoint).")]
    public float speed = 1.5f;

    public void SpeedChange(Flamemove target)
    {
        //If the target is still changing, instantly change the speed to prevent getting stuck in the previous change enumerator
        if (target.isChanging)
        {
            WayPoints.HaltAllSpeedChanges();
        }
        StartCoroutine(Change(target));
    }

    private IEnumerator Change(Flamemove target)
    {
        target.isChanging = true;
        while (!Mathf.Approximately(target.speed, speed))
        {
            target.speed = Mathf.MoveTowards(target.speed, speed, Time.deltaTime * 0.5f);
            yield return null;
        }
        target.isChanging = false;
    }
}
