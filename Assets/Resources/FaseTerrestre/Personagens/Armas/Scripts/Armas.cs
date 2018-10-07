using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armas : MonoBehaviour {

	public UnityEngine.Sprite armaHud;
	public bool maoEsquerda = true;
	public GameObject soundPrefab;
	public AudioClip[] sons;
	public float[] volumes;
	public int armaId;
	public int tipoDeFogo = 0;

	public GameObject municaoPrefab;
	public GameObject claraoPrefab;
	public GameObject carregadorPrefab;
	public int municoesNoPente;
	public int municoesQuardada;
	public int maxMunicao;
	public Transform canoDaArma;

	public float taxaDeTido;
	public float taxaDeTidoAbaixado;
	private float contador;

	public Transform carregadorBone;
	public MudarPai carregador;

	public float dano;
	public float velocidade;

	void Awake () {
		contador = taxaDeTido + 1f;
	}
	
	// Update is called once per frame
	void Update () {
		contador += Time.deltaTime;
	}

	public bool PossoAtirar (bool abaixado) {
		if (abaixado) {
			return municoesNoPente > 0 && contador > taxaDeTidoAbaixado;
		}
		return municoesNoPente > 0 && contador > taxaDeTido;
	}

	public bool TempoDeTiroPassou (bool abaixado) {
		if (abaixado) {
			return contador > taxaDeTidoAbaixado;
		}
		return contador > taxaDeTido;
	}

	public bool PossoRecarregar () {
		return municoesQuardada > 0 && municoesNoPente < maxMunicao;
	}

	public void Recarregar () {
		TocarSom (2);

		if (carregador != null) {
			carregador.MudarMeuPai (carregadorBone);
		}

		if (municoesQuardada == 999) {
			municoesNoPente = maxMunicao;
			return;
		}

		municoesQuardada += municoesNoPente;
		if (municoesQuardada > maxMunicao) {
			municoesNoPente = maxMunicao;
			municoesQuardada -= maxMunicao;
		} else {
			municoesNoPente = municoesQuardada;
			municoesQuardada = 0;
		}
	}

	public void Atirar () {
		contador = 0;

		Municoes temp_municoes = Instantiate (municaoPrefab,canoDaArma.position, canoDaArma.rotation).GetComponent<Municoes> ();
		temp_municoes.dano = dano;
		temp_municoes.velocidade = velocidade;
		Instantiate (claraoPrefab,canoDaArma.position, canoDaArma.rotation);

		municoesNoPente--;

		TocarSom (0);
	}

	public void Atirar (GameObject municao) {
		contador = 0;

		Municoes temp_municoes = Instantiate (municao,canoDaArma.position, canoDaArma.rotation).GetComponent<Municoes> ();
		temp_municoes.dano = dano;
		temp_municoes.velocidade = velocidade / 3.5f;
		Instantiate (claraoPrefab,canoDaArma.position, canoDaArma.rotation);

		municoesNoPente--;

		TocarSom (0);
	}

	public void DroparCarregador (Transform novoPai) {
		if (carregador != null) {
			carregador.MudarMeuPai (novoPai);
			Instantiate (carregadorPrefab, carregadorBone.position, carregadorBone.rotation);
		}
		TocarSom (1);
	}

	public void PuxarFerrolho () {
		TocarSom (3);
	}

	public void EmpurarFerolho () {
		TocarSom (4);
	}

	private void TocarSom (int indice) {
		AudioSource temp_AudioSource = Instantiate (soundPrefab, transform).GetComponent<AudioSource> ();
		temp_AudioSource.clip = sons [indice];
		temp_AudioSource.volume = volumes [indice];

		temp_AudioSource.Play ();
	}
}
