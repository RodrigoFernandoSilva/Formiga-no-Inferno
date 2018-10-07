using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroirClarao : MonoBehaviour {

	public float tempoApagar = 0.5f;
	public float tempoDestoir = 0.5f;

	void Start () {
		StartCoroutine ("ApagarAposTempo");
	}

	IEnumerator ApagarAposTempo () {
		yield return new WaitForSeconds (tempoApagar);
		GetComponentInChildren<SpriteRenderer> ().enabled = false;
		StartCoroutine ("DestroirAposTempo");
	}

	IEnumerator DestroirAposTempo () {
		yield return new WaitForSeconds (tempoDestoir);
		Destroy (gameObject);
	}
}
