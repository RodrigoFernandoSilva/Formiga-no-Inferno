using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TocarSons : MonoBehaviour {

	public GameObject prefabSons;
	private DATA_SONS MUNDO_DATA_SONS;

	// Use this for initialization
	void Awake () {
		MUNDO_DATA_SONS = FindObjectOfType <DATA_SONS> ();
	}
	
	private void TocarSomAndando () {
		TocarSom (Random.Range (0, 3));
	}

	private void TocarSomDano () {
		TocarSom (Random.Range (4, 6));
	}

	private void TocarSom (int indice) {
		AudioSource temp_AudioSource = Instantiate (prefabSons, transform).GetComponent<AudioSource> ();
		temp_AudioSource.clip = MUNDO_DATA_SONS.sons [indice];
		temp_AudioSource.volume = MUNDO_DATA_SONS.volumes [indice];
		temp_AudioSource.Play ();
	}
}
