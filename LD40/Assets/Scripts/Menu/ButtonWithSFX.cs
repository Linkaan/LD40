using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonWithSFX : Button {

	public SFXManager sfxManager;

	protected override void Start () {
		sfxManager = GameObject.FindObjectOfType<SFXManager> ();
		base.Start ();
	}

	public override void OnPointerEnter (PointerEventData eventData) {
		sfxManager.Play (sfxManager.buttonHover);
		base.OnPointerEnter (eventData);
	}

	public override void OnPointerClick(PointerEventData eventData) {
		sfxManager.Play (sfxManager.buttonPress);
		base.OnPointerClick(eventData);
	}
}
