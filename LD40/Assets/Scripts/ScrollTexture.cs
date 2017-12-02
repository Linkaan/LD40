using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

	public float scrollXspeed = 0.1f;
	public float scrollYspeed = 0.1f;

	private Material material;

	void Start () {
		material = GetComponent<Renderer> ().material;
	}
	
	void Update () {
		Vector2 textureOffset = material.GetTextureOffset ("_MainTex");
		textureOffset.x += scrollXspeed * Time.deltaTime;
		textureOffset.y += scrollYspeed * Time.deltaTime;
		material.SetTextureOffset ("_MainTex", textureOffset);
	}
}
