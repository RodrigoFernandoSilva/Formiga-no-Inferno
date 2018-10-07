using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoDeInimigos : MonoBehaviour {

	public GameObject inimiga;
	private int quantidadeCriada = 0;
	public int quantidade;

	private float contador;
	public Vector2 tempoRange;
	public Vector2 distancias;
	public Vector2 tempoTiroRange;

	// Use this for initialization
	void Awake () {
		contador = Random.Range (tempoRange.x, tempoRange.y);;
	}
	
	// Update is called once per frame
	void Update () {
		contador -= Time.deltaTime;

		if (quantidadeCriada < quantidade) {
			if (contador <= 0) {
				quantidadeCriada++;
				contador = Random.Range (tempoRange.x, tempoRange.y);
				SpawInimigo ();
			}
		} else {
			if (FindObjectsOfType<InimigaIA> ().Length == 0) {
				Destroy (gameObject);
			}
		}
	}

	private void SpawInimigo () {
		float distanciaZ = (transform.position - Camera.main.transform.position).z;
		float bordaEsquerda = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanciaZ)).x;
		float bordadireita = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanciaZ)).x;

		Vector3 posicao = Vector3.zero;
		float posicaoRandom;
		if (Random.Range (0, 2) == 0) {
			posicaoRandom = Random.Range (bordadireita - 0.5f, bordadireita - distancias.x);
			posicao = new Vector3 (bordadireita + 2f, transform.position.y, Random.Range (-6f, 6f));
		} else {
			posicaoRandom = Random.Range (bordaEsquerda + 0.5f, bordaEsquerda + distancias.y);
			posicao = new Vector3 (bordaEsquerda - 2f, transform.position.y, Random.Range (-6f, 6f));
		}

		InimigaIA temp_Inimigo = Instantiate (inimiga, posicao, Quaternion.identity).GetComponent<InimigaIA> ();
		temp_Inimigo.MudarPosicaoSorteada (posicaoRandom);
		temp_Inimigo.tempoTiroRange = tempoTiroRange;
	}
}
