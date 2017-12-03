using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour {

	public WinMenu winMenu;

	public void DoReloadScene () {
		StartCoroutine (ReloadScene ());
	}

	public void DoShowWinMenu (float startTime) {
		StartCoroutine (ShowWinMenu (startTime));
	}

	private IEnumerator ReloadScene () {
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	private IEnumerator ShowWinMenu (float startTime) {
		yield return new WaitForSeconds (0.5f);
		winMenu.totalTime = Time.time - startTime;
		winMenu.ShowMenu ();
	}
}
