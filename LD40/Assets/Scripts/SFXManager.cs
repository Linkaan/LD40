using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public AudioSource player;

	public AudioClip buttonHover;
	public AudioClip buttonPress;

	public AudioClip explode;

	public AudioClip destinationReached;

	public void Play(AudioClip sfx) {
		player.pitch = 1;
		player.clip = sfx;
		player.Play ();
	}

	public void Play(AudioClip sfx, float pitch) {
		player.pitch = pitch;
		player.clip = sfx;
		player.Play ();
	}
}
