using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiPlayer : MonoBehaviour {

	public DestinationManager destMgr;

	public GameObject explosionPrefab;

	public GameObject ghostPrefab;

	private NavMeshAgent agent;

	private Destination startDestination;

	private Destination currentDestination;

	private int agentPriority;

	private bool hasInitiated;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = false;
		startDestination = destMgr.nextDestination ();
		this.transform.position = startDestination.transform.position;
		currentDestination = destMgr.nextDestination ();
		hasInitiated = false;
		agentPriority = 0;

		IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Blue);
		IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Yellow);
	}

	void FixedUpdate () {
		if (!hasInitiated) {
			hasInitiated = true;
			agent.enabled = true;
			agent.SetDestination (currentDestination.goalPosition);
		}
		if (agent.pathPending)
			return;
		if (hasInitiated && agent.desiredVelocity.magnitude == 0) {
			Destination dest = destMgr.nextDestination ();

			if (dest == null) {
				Instantiate (explosionPrefab, this.transform);
				GameObject.Destroy (this);
			} else {
				IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Purple);
				IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Purple);
				SpawnGhost ();
				startDestination = currentDestination;
				currentDestination = dest;
				agent.SetDestination (dest.goalPosition);
				IconManager.SetIcon (startDestination.gameObject, IconManager.LabelIcon.Blue);
				IconManager.SetIcon (currentDestination.gameObject, IconManager.LabelIcon.Yellow);
			}
		}	
	}

	private void SpawnGhost () {
		Debug.Log ("spawned");
		Ghost newGhost = Instantiate (ghostPrefab, startDestination.goalPosition, Quaternion.identity).GetComponent<Ghost>();
		agentPriority += 10;
		newGhost.Init (startDestination, currentDestination, agentPriority);
	}
}
