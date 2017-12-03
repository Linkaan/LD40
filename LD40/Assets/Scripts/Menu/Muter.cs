using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Muter : MonoBehaviour {

	public AudioSource[] players;

	public Image muteButtonImage;

	public Sprite muted_sprite;
	public Sprite unmuted_sprite;

	public bool isMuted = false;

	public void ToggleMute () {
		isMuted = !isMuted;
		muteButtonImage.sprite = isMuted ? muted_sprite : unmuted_sprite;
		foreach (AudioSource player in players) {
			player.mute = isMuted;
		}
	}
}
