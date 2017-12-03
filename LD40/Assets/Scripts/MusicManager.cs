using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioSource player;

	public AudioClip[] musicPlaylist;

	void Update () {
		if (!player.isPlaying) {
			player.clip = musicPlaylist [Random.Range (0, musicPlaylist.Length)];
			player.Play ();
		}
	}
}
