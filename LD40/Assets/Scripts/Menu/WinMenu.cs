using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinMenu : MonoBehaviour {

	public float totalTime;

	public TextMeshProUGUI scoreText;

	public void ShowMenu () {
		scoreText.text = string.Format ("You finished the level in {0} seconds", (int) totalTime);
		gameObject.SetActive (true);
	}

	public void NextLevel () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
