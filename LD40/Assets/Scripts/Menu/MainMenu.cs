using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public EnemyAppear appearer;

	public GameObject levelMenu;

	public void ShowLevel () {
		appearer.Appear (onReady());
	}

	public void QuitGame () {
		Debug.Log ("quit!");
		Application.Quit ();
	}

	private IEnumerator onReady () {
		levelMenu.SetActive (true);
		yield return null;
	}
}
