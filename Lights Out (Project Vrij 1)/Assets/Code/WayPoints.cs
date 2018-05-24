using UnityEngine;

public class WayPoints : MonoBehaviour {

	public Transform[] points;

	void Awake(){
		points = new Transform[transform.childCount];
		for(int i = 0; i < points.Length; i++)
		{
			points[i] = transform.GetChild (i);	
		}
	}

    public static void HaltAllSpeedChanges()
    {
        WaypointSpeedChanger[] changers = FindObjectsOfType<WaypointSpeedChanger>();
        for (int i = 0; i < changers.Length; i++)
        {
            Debug.LogWarning("Stopped change Coroutine on " + changers[i].name);
            changers[i].StopAllCoroutines();
        }
    }
}
