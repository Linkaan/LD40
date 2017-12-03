using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Vector3 goalPosition;

	public bool hasReached = false;

	public bool canDelete = false;

	void Start () {
		this.goalPosition = this.transform.position;	
	}

}
