using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShowPath : MonoBehaviour {

	private Transform target; //to hold the transform of the target

	private LineRenderer line; //to hold the line Renderer

	void Start () {
		line = GetComponent<LineRenderer> (); //get the line renderer
	}

	public void hidePath () {
		line.enabled = false;
	}

	public void setTarget(Transform target) {
		this.target = target;
	}

	public void displayPath (NavMeshPath path) {
		if (path == null)
			return;

		Vector3 originPosition = transform.position;
		originPosition.y = target.position.y;
		line.SetPosition (0, originPosition); //set the line's origin

		if (path.corners.Length < 2) {
			line.positionCount = 2;
			line.SetPosition (1, target.position);
			return;
		}

		line.positionCount = path.corners.Length; //set the array of positions to the amount of corners

		for (int i = 1; i < path.corners.Length; i++){
			line.SetPosition (i, path.corners[i]); //go through each corner and set that to the line renderer's position
		}
	}
}
