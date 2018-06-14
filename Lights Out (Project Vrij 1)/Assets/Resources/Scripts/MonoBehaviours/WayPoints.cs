using UnityEngine;

[ExecuteInEditMode]
public class WayPoints : MonoBehaviour {

	public Transform[] points;
    public bool liveUpdate = false;

    void Awake(){
		points = new Transform[transform.childCount];
		for(int i = 0; i < points.Length; i++)
		{
			points[i] = transform.GetChild (i);	
		}
	}

    private void Update()
    {
        if (liveUpdate && transform.childCount != points.Length)
        {
            points = new Transform[transform.childCount];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.GetChild(i);
            }
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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = new Color((float)i / points.Length, (float)i / points.Length, 1, 1);
            Gizmos.DrawWireSphere(points[i].position, 0.2f);
            if (i + 1 < points.Length)
                Gizmos.DrawLine(points[i + 1].position, points[i].position);
        }
    }
}
