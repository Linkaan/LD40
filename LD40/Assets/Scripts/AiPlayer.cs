using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiPlayer : MonoBehaviour {

	public DestinationManager destMgr;

	public ArrowManager arrowMgr;

	public GameObject explosionPrefab;

	public GameObject ghostPrefab;

	private NavMeshAgent agent;

	private Destination startDestination;

	private Destination currentDestination;

	private ShowPath pathShower;

	private int agentPriority;

	private bool hasInitiated;

	void Start () {
		pathShower = GetComponent<ShowPath> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
		startDestination = destMgr.nextDestination ();
		this.transform.position = startDestination.transform.position;
		currentDestination = destMgr.nextDestination ();
		hasInitiated = false;
		agentPriority = 0;
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
			if (agent.desiredVelocity.magnitude == 0) {
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
		Ghost newGhost = Instantiate (ghostPrefab, startDestination.goalPosition, Quaternion.identity).GetComponent<Ghost>();
		agentPriority += 10;
		newGhost.Init (startDestination, currentDestination, agentPriority);
	}
}
