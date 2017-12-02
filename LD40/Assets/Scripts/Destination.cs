using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {

	public Vector3 goalPosition;

	public bool hasReached = false;

	void Start () {
		this.goalPosition = this.transform.position;	
	}

}
