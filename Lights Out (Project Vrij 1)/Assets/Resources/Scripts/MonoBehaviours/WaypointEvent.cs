using UnityEngine;

public abstract class WaypointEvent : MonoBehaviour {

    //Waypoint events are called whenever the energy source reaches the waypoint. Abstract, so must be overridden.
    public abstract void Execute(FlameBehaviour flame);
	
}