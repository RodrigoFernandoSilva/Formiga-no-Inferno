using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InimigaIA : MonoBehaviour {

	public GameObject[] caixasPrefab;
	public GameObject municaoprefab;
	private Rigidbody2D meuRigidbody2D;
	public GameObject[] armas;
	public Transform maoEsquerda;
	public Transform maoDireita;
	private Armas minhaArma;
	private Transform jose;
	private Animator meuAnimator;
	public float minhaVida = 100f;
	private float posicaoSorteada;

	private bool recarregando;
	private bool chegou = false;
	private float velocidade;
	public float tempoAndar;
	public float tempoParar;
	public float velo_Andando;

	private bool morto;
	private int direcao;

	private float larguraInicial;

	private float tempoDeTiro;
	public Vector2 tempoTiroRange;
	private float contador;

	// Use this for initialization
	void Awake () {
		contador = Random.Range (tempoTiroRange.x, tempoTiroRange.y);

		meuRigidbody2D = GetComponent<Rigidbody2D> ();

		recarregando = false;
		morto = false;

		meuAnimator = GetComponentInChildren<Animator> ();
		jose = GameObject.FindWithTag ("Player").transform;

		larguraInicial = transform.localScale.x;

		//Sorteia e cria uma arma para o inimigo
		minhaArma = Instantiate (armas [Random.Range (0, 3)], transform).GetComponent<Armas> ();
		minhaArma.enabled = true;

		//Coloca a arma na mão certa
		if (minhaArma.maoEsquerda) {
			minhaArma.GetComponent<MudarPai> ().MudarMeuPai (maoEsquerda);
		} else {
			minhaArma.GetComponent<MudarPai> ().MudarMeuPai (maoDireita);
		}

		meuAnimator.SetInteger ("armaId", minhaArma.armaId);

		//Colocar a arma criada no mesmo layer na mão dele, posi os inimigos estão em uma orderm de layer diferente da do jogador
		SpriteRenderer[] imagens = minhaArma.GetComponentsInChildren <SpriteRenderer> ();
		for (int i = 0; i < imagens.Length; i++) {
			imagens [i].sortingOrder = -3;
		}
	}

	// Update is called once per frame
	void Update () {
		Morrer ();

		if (morto) {
			return;
		}

		if (!chegou) {
			velocidade = Mathf.Lerp (velocidade, velo_Andando * direcao, tempoAndar * Time.deltaTime);

			if (direcao < 0) {
				if (transform.position.x < posicaoSorteada) {
					chegou = true;
				}
			} else {
				if (transform.position.x > posicaoSorteada) {
					chegou = true;
				}
			}

			if (chegou) {
				if (Random.Range (0, 2) == 0) {
					meuAnimator.SetBool ("abaixado", true);
				}
			}
		} else {
			velocidade = Mathf.Lerp (velocidade, 0f, tempoParar * Time.deltaTime);
		}

		meuAnimator.SetFloat ("velocidade", Mathf.Abs (meuRigidbody2D.velocity.x));

		Atirar ();
		Flip ();
	}

	void FixedUpdate () {
		if (meuRigidbody2D != null) {
			meuRigidbody2D.velocity = new Vector2 (velocidade, meuRigidbody2D.velocity.y);
		}
	}

	private void Atirar () {
		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (3);
		if (minhaArma.PossoAtirar (true) && !recarregando && temp_AnimatorStateInfo.IsTag ("Vazio")) {
			contador -= Time.deltaTime;

			if (contador <= 0) {
				minhaArma.Atirar (municaoprefab);
				contador = Random.Range (tempoTiroRange.x, tempoTiroRange.y);
				meuAnimator.SetTrigger ("atirar");
			}
		}

		if (temp_AnimatorStateInfo.IsTag ("Vazio") && !recarregando &&
			minhaArma.municoesNoPente <= 0 && minhaArma.TempoDeTiroPassou (true)) {

			minhaArma.municoesQuardada += minhaArma.maxMunicao;
			meuAnimator.SetTrigger ("recarregar");
			recarregando = true;
			StartCoroutine ("Recarregar");
		}
	}

	private void Morrer () {
		if (minhaVida < 1) {
			//Dispara a animação de Morrer
			if (!morto) {
				minhaArma.GetComponent<MudarPai> ().MudarMeuPai (null);
				meuAnimator.SetTrigger ("Morrer");
				morto = true;

				minhaArma.gameObject.AddComponent (typeof(Rigidbody2D));

				BoxCollider2D[] temp_boxCol2D = minhaArma.GetComponentsInChildren<BoxCollider2D> ();
				for (int i = 0; i < temp_boxCol2D.Length; i++) {
					temp_boxCol2D [i].enabled = true;
				}

				Destroy (GetComponent<Collider2D> ());
				Destroy (GetComponent<Rigidbody2D> ());

				if (Random.Range (0f, 100f) <= 35) {
					Vector3 posicao = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
					Instantiate (caixasPrefab [Random.Range (0, caixasPrefab.Length)], posicao, transform.rotation);
				}
			} else {
				AnimatorStateInfo temp_State = meuAnimator.GetCurrentAnimatorStateInfo (0);

				//Caso o tag da animação seja igual a <Morto>, quer dizer q os coponentes deste inimigo pode ser deletados
				if (temp_State.IsTag ("Morto")) {
					Destroy (meuAnimator);
					Destroy (minhaArma.GetComponent<MudarPai> ());
					Destroy (minhaArma);
					Destroy (GetComponent<InimigaIA> ());
				}
			}
			return;
		}
	}

	/// <summary>
	/// Faz o inimigo e o cano da sua arma sempre apontar em direção ao jogador
	/// </summary>
	private void Flip () {
		//Faz o cano da arma apontar de acordo pra onde o inimigo esta olhando, pois a escala em x do inimigo pode ficar negativa
		if (transform.localScale.x > 0) {
			minhaArma.canoDaArma.localEulerAngles = new Vector3 (0f, 0f, 0f);
		} else {
			minhaArma.canoDaArma.localEulerAngles = new Vector3 (0f, 0f, 180f);
		}

		if (chegou) { 
			//Faz o inimigo olhar sempre em direção ao jogador
			if (transform.localScale.x > 0 && transform.position.x > jose.position.x) {
				transform.localScale = new Vector3 (-larguraInicial, transform.localScale.y, transform.localScale.z);
			} else if (transform.localScale.x < 0 && transform.position.x < jose.position.x) {
				transform.localScale = new Vector3 (larguraInicial, transform.localScale.y, transform.localScale.z);
			}
		} else {
			//Faz o inimigo apontar para a direção em q ele estiver se movendo
			if (transform.localScale.x > 0 && meuRigidbody2D.velocity.x < 0) {
				transform.localScale = new Vector3 (-larguraInicial, transform.localScale.y, transform.localScale.z);
			} else if (transform.localScale.x < 0 && meuRigidbody2D.velocity.x > 0) {
				transform.localScale = new Vector3 (larguraInicial, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	public void PerderVida (float dano) {
		if (jose.position.x < transform.position.x && transform.localScale.x < 0) {
			meuAnimator.SetTrigger ("levarDanoFrente");
		} else if (jose.position.x > transform.position.x && transform.localScale.x > 0) {
			meuAnimator.SetTrigger ("levarDanoFrente");
		} else{
			meuAnimator.SetTrigger ("levarDanoCostas");
		}

		if (dano == 50) {
			minhaVida = 0;
		}

		minhaVida -= dano;
	}

	public void MudarPosicaoSorteada (float valor) {
		posicaoSorteada = valor;

		if (transform.position.x < valor) {
			direcao = 1;
		} else {
			direcao = -1;
		}
	}

	IEnumerator Recarregar () {
		yield return new WaitForSeconds (2f);
		recarregando = false;
	}

	public void PorMunicao () {
		minhaArma.Recarregar ();
	}

	public void DroparCarregador () {
		minhaArma.DroparCarregador (maoDireita);
	}

	public void PuxarFerrolho () {
		minhaArma.PuxarFerrolho ();
	}

	public void EmpurarFerrolho () {
		minhaArma.EmpurarFerolho ();
	}
}
