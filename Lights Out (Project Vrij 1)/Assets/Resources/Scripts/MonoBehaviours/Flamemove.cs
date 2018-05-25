﻿using UnityEngine;

public class Flamemove : MonoBehaviour {

	public float speed = 1.5f;

    public bool isChanging { get; set; }
    public bool isRunning { get; set; }

	private Transform target;
	private int waypointIndex = 0;
    private WayPoints wayPoints;
    private Rigidbody rb;
    private PlayerController player;

	void Start()
	{
        player = FindObjectOfType<PlayerController>();
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
            CheckPlayerDistance();
            return;
        }
		Vector3 dir = target.position - transform.position;
		transform.Translate (dir.normalized  * speed * Time.deltaTime, Space.World);
        ApplyInertia();
		if (Vector3.Distance (transform.position, target.position) <= 0.6f) 
		{
			GetNextWaypoint ();
		}
	}

    void CheckPlayerDistance()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {
            isRunning = true;
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
		waypointIndex++;
        if (wayPoints.points[waypointIndex].GetComponent<WaypointSpeedChanger>())
        {
            wayPoints.points[waypointIndex].GetComponent<WaypointSpeedChanger>().SpeedChange(this);
        }
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
