using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAppear : MonoBehaviour {

	public GameObject spawnEffectPrefab;

	public Transform destination;

	public Transform origin;

	private NavMeshAgent agent;

	private bool hasArrived = false;

	private bool hasDestination = false;

	private IEnumerator onReady;

	void Update () {
		if (hasArrived || !hasDestination || agent.pathPending)
			return;
		if (agent.remainingDistance < 1 && agent.desiredVelocity.magnitude == 0) {
			hasArrived = true;
			StartCoroutine (onReady);
		}
	}

	public void Reset () {
		hasArrived = false;
		hasDestination = false;
		agent.enabled = false;
		transform.position = origin.position;
		transform.rotation = Quaternion.identity;
		gameObject.SetActive (false);
	}

	public void Appear (IEnumerator onReady) {
		this.onReady = onReady;
		gameObject.SetActive (true);
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = true;
		GameObject go = Instantiate (spawnEffectPrefab, this.transform.position, Quaternion.identity);
		GameObject.Destroy (go, 2f);
		//StartCoroutine (FadeIn (3.0f, transform.Find("Graphics").GetComponent<Renderer> ().material));
		StartCoroutine (WalkBehind ());
	}

	private IEnumerator WalkBehind () {
		yield return new WaitForSeconds (0.2f);
		agent.SetDestination (destination.position);
		hasDestination = true;
	}

}
