using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AcharCamera : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponentInChildren<Canvas> ().worldCamera = FindObjectOfType<Camera> (); //Coloca a cemra principal do jogo no "event camera"
	}
}
