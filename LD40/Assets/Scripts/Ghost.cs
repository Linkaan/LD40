using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour {
	/*
	public Destination pointA;
	public Destination pointB;
*/
	public GameObject spawnEffectPrefab;

	public float waypointResolutionRadius = 2;

	public LayerMask waypointLayerMask;

	private NavMeshAgent agent;

	//private Destination currentDestination;

	private List<Waypoint> waypoints;

	private int waypointIndex;

	private bool goingForward;

	private bool hasInitialized;

	public void Init (Waypoint pointA, Waypoint pointB, int priority, List<Waypoint> waypoints) {
		//this.pointA = pointA;
		//this.pointB = pointB;
		this.waypoints = FilterWaypoints(waypoints, pointA, pointB);
		waypointIndex = 0;
		goingForward = true;

		hasInitialized = false;
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
		agent.transform.position = pointA.transform.position;

		//agent.Warp(pointA.goalPosition);
		agent.avoidancePriority = priority;
		agent.stoppingDistance = 0.5f;

		GameObject go = Instantiate (spawnEffectPrefab, this.transform.position, Quaternion.identity);
		GameObject.Destroy (go, 2f);
	}

	List<Waypoint> FilterWaypoints(List<Waypoint> waypoints, Waypoint pointA, Waypoint pointB) {
		List<Waypoint> beforeDeletion = new List<Waypoint> (waypoints);
		List<Waypoint> ignore = new List<Waypoint> ();
		ignore.Add (pointA);
		ignore.Add (pointB);
		List<Waypoint> filtered = new List<Waypoint> ();
		RemoveOtherWaypoints (filtered, waypoints, ignore, pointA);
		RemoveOtherWaypoints (filtered, waypoints, ignore, pointB);
		while (waypoints.Count > 0) {
			Waypoint point = waypoints [0];
			RemoveOtherWaypoints (filtered, waypoints, ignore, point);
			waypoints.RemoveAt (0);
			filtered.Add (point);
			IconManager.SetIcon (point.gameObject, IconManager.Icon.DiamondTeal);
		}
		filtered.Insert (0, pointA);
		filtered.Add (pointB);
		IconManager.SetIcon (pointA.gameObject, IconManager.Icon.DiamondRed);
		IconManager.SetIcon (pointB.gameObject, IconManager.Icon.DiamondRed);
		while (beforeDeletion.Count > 0) {
			Waypoint point = beforeDeletion [0];
			beforeDeletion.RemoveAt (0);
			if (point.canDelete || !filtered.Contains (point)) {
				GameObject.Destroy (point.gameObject);
			}
		}
		return filtered;
	}

	void RemoveOtherWaypoints(List<Waypoint> previous, List<Waypoint> waypoints, List<Waypoint> ignore, Waypoint point) {
		Collider[] colliders = Physics.OverlapSphere (point.transform.position, waypointResolutionRadius, waypointLayerMask);
		foreach (Collider collider in colliders) {
			Waypoint otherPoint = collider.GetComponent<Waypoint> ();
			if (otherPoint != point && !ignore.Contains(otherPoint)) {
				if (previous.Contains(otherPoint)) {
					previous.Remove(otherPoint);
				}
				waypoints.Remove (otherPoint);
				otherPoint.canDelete = true;
				//GameObject.Destroy (otherPoint.gameObject);
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.transform.CompareTag("Player")) {
			collision.gameObject.GetComponent<Player> ().CollidedWithEnemy ();
		}
	}

	void FixedUpdate () {
		if (!hasInitialized) {
			hasInitialized = true;
			agent.enabled = true;
			agent.SetDestination (NextWaypoint().transform.position);
		}
		if (!hasInitialized || !agent || agent.pathPending)
			return;
		if (agent.remainingDistance < agent.stoppingDistance) {
			//currentDestination = (currentDestination == pointB ? pointA : pointB);
			//agent.SetDestination (currentDestination.goalPosition);
			agent.SetDestination (NextWaypoint().transform.position);
		}
	}

	Waypoint NextWaypoint() {
		Waypoint point = waypoints [waypointIndex];
		if (goingForward) {
			waypointIndex++;
			if (waypointIndex >= waypoints.Count) {
				waypointIndex = waypoints.Count - 1;
				goingForward = false;
			}
		} else {
			waypointIndex--;
			if (waypointIndex < 0) {
				waypointIndex = 0;
				goingForward = true;
			}
		}
		return point;
	}
	
}
