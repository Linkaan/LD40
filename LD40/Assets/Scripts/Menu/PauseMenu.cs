using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenu;

	private bool isPaused = false;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (!isPaused)
				Pause ();
			else
				Resume ();
		}
	}

	public void Pause () {
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
	}

	public void Resume () {
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
	}

	public void MainMenu () {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
	}
}
