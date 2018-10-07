using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CanvasControles : MonoBehaviour {

	public int indiceTutorial;

	public string[] nomeDaPasta;

	public Text textoComoFazer;
	public Image gif;

	private int indiceGif;

	public float maxTempoMudar;
	private float tempoMudar;

	// Use this for initialization
	void Awake () {
		tempoMudar = 0;
		indiceTutorial = -1;
		MudarIndice (0);

		gif.sprite = CarregarResourcesImage ();
	}
	
	// Update is called once per frame
	void Update () {
		tempoMudar += Time.deltaTime;

		if (tempoMudar > maxTempoMudar) {
			indiceGif++;
			Sprite temp = CarregarResourcesImage ();

			if (temp == null) {
				indiceGif = 0;
				temp = CarregarResourcesImage ();
			}

			gif.sprite = CarregarResourcesImage ();
			tempoMudar = 0f;
		}
	}

	private Sprite CarregarResourcesImage () {
		return Resources.Load  <Sprite> ("FaseTerrestre/Menu/Tutorial/" + nomeDaPasta[indiceTutorial] + "/" + nomeDaPasta[indiceTutorial] + indiceGif);
	}

	public void MudarIndice (int novoIndice) {
		if (indiceTutorial == novoIndice) {
			return;
		}

		indiceTutorial = novoIndice;

		tempoMudar = 0f;
		indiceGif = 0;
		MudarTextoComoFazer ();
	}

	private void MudarTextoComoFazer () {
		string texto =  Resources.Load  <TextAsset> ("FaseTerrestre/Menu/Tutorial/" + nomeDaPasta[indiceTutorial] + "/ComoFazer").text;
		textoComoFazer.text = nomeDaPasta[indiceTutorial] + ":\n\t" + texto;
	}
}
