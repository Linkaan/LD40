using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiPlayer : MonoBehaviour {

	public DestinationManager destMgr;

	public ArrowManager arrowMgr;

	public GameObject explosionPrefab;

	public GameObject ghostPrefab;

	public Transform waypointsHolder;

	private NavMeshAgent agent;

	private Destination startDestination;

	private Destination currentDestination;

	private ShowPath pathShower;

	private int agentPriority;

	private bool hasInitiated;

	private List<Waypoint> waypoints;

	private float lastTime;

	void Start () {
		pathShower = GetComponent<ShowPath> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
		startDestination = destMgr.nextDestination ();
		this.transform.position = startDestination.transform.position;
		currentDestination = destMgr.nextDestination ();
		hasInitiated = false;
		agentPriority = 0;

		waypoints = new List<Waypoint> ();
	}

	void FixedUpdate () {
		if (!hasInitiated) {
			hasInitiated = true;
			agent.enabled = true;
			agent.SetDestination (currentDestination.goalPosition);
			onNewDestination ();
		}
		if (agent.pathPending)
			return;
		if (hasInitiated) {
			if (agent.remainingDistance < agent.stoppingDistance) {
				Destination dest = destMgr.nextDestination ();

				if (dest == null) {
					Instantiate (explosionPrefab, this.transform.position, Quaternion.identity);
					arrowMgr.HideArrow ();
					pathShower.hidePath ();
					GameObject.Destroy (this.gameObject);
				} else {
					IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Purple);
					IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Purple);
					SpawnGhost ();
					startDestination = currentDestination;
					currentDestination = dest;
					agent.SetDestination (dest.goalPosition);

					onNewDestination ();
				}
			}
				
			if (Time.time - lastTime > 1.0f) {
				lastTime = Time.time;
				Waypoint point = new Waypoint ();
				point.goalPosition = transform.position;
				waypoints.Add (point);
			}

			pathShower.displayPath (agent.path);
		}
	}

	private void onNewDestination() {
		pathShower.setTarget (currentDestination.transform);
		IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Blue);
		IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Yellow);
		arrowMgr.PointAtPosition (currentDestination.goalPosition);
	}

	private void SpawnGhost () {
		/*
		Waypoint point = new Waypoint ();
		point.goalPosition = transform.position;
		waypoints.Add (point);

		Ghost newGhost = Instantiate (ghostPrefab, startDestination.goalPosition, Quaternion.identity).GetComponent<Ghost>();
		agentPriority += 10;
		newGhost.Init (startDestination, currentDestination, agentPriority, new List<Waypoint>(waypoints));
		waypoints.Clear ();
		*/
	}
}
