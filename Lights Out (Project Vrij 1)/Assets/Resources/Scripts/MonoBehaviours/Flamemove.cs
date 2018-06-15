using UnityEngine;

public class Flamemove : MonoBehaviour {

	public float speed = 1.5f;
    public float moveThreshold = 10f;
    
    public bool isChanging { get; set; }
    public bool isRunning { get; set; }

	private Transform target;
	private int waypointIndex = 0;
    private WayPoints wayPoints;
    private Rigidbody rb;
    private FlameBehaviour flame;

	void Start()
	{
        flame = GetComponentInChildren<FlameBehaviour>();
        flame.vitality = 0;
        isChanging = false;
        isRunning = false;
        wayPoints = FindObjectOfType<WayPoints>();
        target = wayPoints.points [0];
        rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
        if (!isRunning)
        {
            if (flame.vitality < 90)
                return;
            else
                isRunning = true;
        }
		Vector3 dir = target.position - transform.position;
        float moveSpeed = flame.vitality > moveThreshold ? speed : 0;
		transform.Translate (dir.normalized  * moveSpeed * Time.deltaTime, Space.World);
        ApplyInertia();
		if (Vector3.Distance (transform.position, target.position) <= 0.6f) 
		{
			GetNextWaypoint ();
		}
	}

	void GetNextWaypoint()
	{

		if (waypointIndex >= wayPoints.points.Length - 1) 
		{
			waypointIndex = waypointIndex - wayPoints.points.Length;
			//if you want it to end as soon as it reaches its final destination
			//Destroy (gameObject);
			return;
		}
        //Check for events
        if (wayPoints.points[waypointIndex].GetComponent<WaypointEvent>())
        {
            wayPoints.points[waypointIndex].GetComponent<WaypointEvent>().Execute(flame);
        }
		waypointIndex++;
        //Check for speed changers
        if (wayPoints.points[waypointIndex].GetComponent<WaypointSpeedChanger>())
        {
            wayPoints.points[waypointIndex].GetComponent<WaypointSpeedChanger>().SpeedChange(this);
        }
        //Set new target
        target = wayPoints.points [waypointIndex];
	}

    void ApplyInertia()
    {
        if (transform.position.y > target.position.y && rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.up);
        }
        if (transform.position.y < target.position.y && rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down);
        }
    }

}
