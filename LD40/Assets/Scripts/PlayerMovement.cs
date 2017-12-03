using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {

	public float movePower = 5f;
	public float rotateSpeed = 200f;
	public float maxRotationInertia = 1000f;

	public AudioSource sfxPlayer;

	public AudioClip walkSFX;

	private Vector3 moveDirection;
	private Vector3 forward;
	private Vector3 right;

	private CharacterController controller;

	void Start () {
		moveDirection = Vector3.zero;
		forward = Vector3.zero;
		right = Vector3.zero;
		controller = GetComponent<CharacterController> ();
	}

	void Update () {
		forward = transform.forward;
		right = new Vector3 (forward.z, 0, -forward.x);

		float horizontalInput = Input.GetAxisRaw ("Horizontal");
		float verticalInput = Input.GetAxisRaw ("Vertical");

		Vector3 targetDirection = horizontalInput * right + verticalInput * forward;

		moveDirection = Vector3.RotateTowards (moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, maxRotationInertia);

		Vector3 movement = moveDirection * Time.deltaTime * movePower;

		controller.Move(movement);

		if (verticalInput == 0) {
			if (sfxPlayer.isPlaying)
				sfxPlayer.Stop ();
		} else if (!sfxPlayer.isPlaying) {
			sfxPlayer.clip = walkSFX;
			sfxPlayer.Play ();
		}

		if (targetDirection != Vector3.zero)
		{
			if (verticalInput >= 0) {
				transform.rotation = Quaternion.LookRotation (moveDirection);
			} else {
				transform.rotation = Quaternion.LookRotation (-moveDirection);
			}
		}
	}
}
