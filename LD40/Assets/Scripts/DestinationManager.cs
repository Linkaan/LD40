using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationManager : MonoBehaviour {

	public GameObject destinationPrefab;

	public List<Destination> destinations;

	public LayerMask destinationLayerMask;

	public int wantedDestinationAmount = 10;
	public int maxAttempts = 10;
	public int destinationSeparationRadius = 5;

	public int maxDestinationHeight = 2;

	public int destinationIndex = 0;

	private Vector3 minBoundsPoint;
	private Vector3 maxBoundsPoint;
	private float boundsSize;

	void Awake () {
		Random.InitState (0);

		boundsSize = float.NegativeInfinity;

		int attempts = 0;
		while (destinations.Count < wantedDestinationAmount && attempts < maxAttempts) {
			Vector3 targetPoint = GetRandomTargetPoint ();
			if (targetPoint.y > maxDestinationHeight || Physics.OverlapSphere (targetPoint, destinationSeparationRadius, destinationLayerMask).Length > 0) {
				attempts++;
			} else {
				Destination newDest = GameObject.Instantiate (destinationPrefab, targetPoint, Quaternion.identity).GetComponent<Destination>();
				attempts = 0;
				destinations.Add (newDest);
			}
		}
	}

	public Destination nextDestination() {
		if (destinationIndex == destinations.Count)
			return null;
		else
			return destinations[destinationIndex++];
	}

	private Vector3 GetRandomTargetPoint() {
		if (boundsSize < 0) {
			minBoundsPoint = Vector3.one * float.PositiveInfinity;
			maxBoundsPoint = -minBoundsPoint;
			var verticies = NavMesh.CalculateTriangulation ().vertices;
			foreach (var point in verticies) {
				if (minBoundsPoint.x > point.x)
					minBoundsPoint = new Vector3 (point.x, minBoundsPoint.y, minBoundsPoint.z);
				if (minBoundsPoint.y > point.y)
					minBoundsPoint = new Vector3 (minBoundsPoint.x, point.y, minBoundsPoint.z);
				if (minBoundsPoint.z > point.z)
					minBoundsPoint = new Vector3 (minBoundsPoint.x, minBoundsPoint.y, point.z);

				if (maxBoundsPoint.x < point.x)
					maxBoundsPoint = new Vector3 (point.x, maxBoundsPoint.y, maxBoundsPoint.z);
				if (maxBoundsPoint.y < point.y)
					maxBoundsPoint = new Vector3 (maxBoundsPoint.x, point.y, maxBoundsPoint.z);
				if (maxBoundsPoint.z < point.z)
					maxBoundsPoint = new Vector3 (maxBoundsPoint.x, maxBoundsPoint.y, point.z);
			}
			boundsSize = Vector3.Distance (minBoundsPoint, maxBoundsPoint);
		}
		var randomPoint = new Vector3 (
			Random.Range(minBoundsPoint.x, maxBoundsPoint.x),
			Random.Range(minBoundsPoint.y, maxBoundsPoint.y),
			Random.Range(minBoundsPoint.z, maxBoundsPoint.z)
		);
		NavMeshHit hit;
		NavMesh.SamplePosition (randomPoint, out hit, boundsSize, 1);
		return hit.position;
	}

}
