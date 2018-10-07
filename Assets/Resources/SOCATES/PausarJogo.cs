using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausarJogo : MonoBehaviour {

	private bool jogoPausado = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			jogoPausado = !jogoPausado;

			if (jogoPausado) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}
		}
	}
}
