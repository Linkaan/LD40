using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour {

	public Destination pointA;
	public Destination pointB;

	private NavMeshAgent agent;

	private Destination currentDestination;

	public void Init (Destination pointA, Destination pointB, int priority) {
		this.pointA = pointA;
		this.pointB = pointB;
		agent = GetComponent<NavMeshAgent> ();
		agent.transform.position = pointA.transform.position;
		agent.avoidancePriority = priority;
		agent.SetDestination (pointB.goalPosition);
	}

	void FixedUpdate () {
		if (!agent || agent.pathPending)
			return;
		if (agent.desiredVelocity.magnitude == 0) {
			currentDestination = (currentDestination == pointB ? pointA : pointB);
			agent.SetDestination (currentDestination.goalPosition);
		}
	}
}
