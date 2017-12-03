using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public DestinationManager destMgr;

	public ArrowManager arrowMgr;

	public GameObject explosionPrefab;

	public GameObject ghostPrefab;

	public GameObject waypointPrefab;

	public Transform waypointsHolder;

	public SceneReloader sceneReloader;

	public SFXManager sfxManager;

	private NavMeshAgent agent;

	private Destination startDestination;

	private Destination currentDestination;

	private ShowPath pathShower;

	private int agentPriority;

	private bool hasInitialized;

	private List<Waypoint> waypoints;

	private float lastTime;

	private float startTime;

	void Start () {
		pathShower = GetComponent<ShowPath> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
		startDestination = destMgr.nextDestination ();
		this.transform.position = startDestination.transform.position;
		currentDestination = destMgr.nextDestination ();
		hasInitialized = false;
		agentPriority = 0;

		startTime = Time.time;

		//agent.updatePosition = false;
		agent.updateRotation = false;
		agent.stoppingDistance = 2f;

		waypoints = new List<Waypoint> ();
	}

	void FixedUpdate () {
		if (!hasInitialized) {
			hasInitialized = true;
			agent.enabled = true;
			onNewDestination ();
		}
		if (agent.pathPending  || !hasInitialized)
			return;
		
		if (agent.remainingDistance < agent.stoppingDistance) {
			Destination dest = destMgr.nextDestination ();

			if (dest == null) {
				sceneReloader.DoShowWinMenu (startTime);
				Explode ();
			} else {
				currentDestination.hasReached = true;
				IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Purple);
				IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Purple);
				SpawnGhost ();
				startDestination = currentDestination;
				currentDestination = dest;

				float pitch = 0.5f + (0.5f / (destMgr.destinations.Count - 2)) * (destMgr.destinationIndex - 1);
				sfxManager.Play (sfxManager.destinationReached, pitch);

				onNewDestination ();
			}
		}

		if (Time.time - lastTime > 1.0f) {
			lastTime = Time.time;
			AddWaypoint (transform.position);
		}

		pathShower.displayPath (agent.path);
	}

	public void CollidedWithEnemy () {
		sceneReloader.DoReloadScene ();
		Explode ();
	}

	private void Explode () {
		Instantiate (explosionPrefab, this.transform.position, Quaternion.identity);
		arrowMgr.HideArrow ();
		pathShower.hidePath ();
		GameObject.Destroy (this.gameObject);
		sfxManager.Play (sfxManager.explode);
	}

	private void onNewDestination () {
		agent.SetDestination (currentDestination.goalPosition);
		agent.isStopped = true;
		pathShower.setTarget (currentDestination.transform);
		IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Blue);
		IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Yellow);
		arrowMgr.PointAtPosition (currentDestination.goalPosition);
	}

	private void SpawnGhost () {
		Ghost newGhost = Instantiate (ghostPrefab, startDestination.goalPosition, Quaternion.identity).GetComponent<Ghost>();
		agentPriority += 10;
		Waypoint pointA = CreateWaypoint (startDestination.goalPosition);
		Waypoint pointB = CreateWaypoint (currentDestination.goalPosition);
		newGhost.Init (pointA, pointB, agentPriority, new List<Waypoint>(waypoints));
		waypoints.Clear ();
	}

	private void AddWaypoint (Vector3 position) {
		Waypoint newWaypoint = CreateWaypoint (position);
		waypoints.Add (newWaypoint);
	}

	private Waypoint CreateWaypoint (Vector3 position) {
		Waypoint newWaypoint = Instantiate (waypointPrefab, position, Quaternion.identity).GetComponent<Waypoint>();
		newWaypoint.transform.SetParent (waypointsHolder);
		return newWaypoint;
	}
}
