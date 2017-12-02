using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour {

	private GameObject arrowObject;

	void Awake () {
		arrowObject = transform.Find ("arrowAnimation").gameObject;
	}

	public void HideArrow() {
		arrowObject.SetActive (false);
	}
	
	public void PointAtPosition(Vector3 position) {
		arrowObject.SetActive (true);
		transform.position = new Vector3 (position.x, transform.position.y, position.z);
	}
}
