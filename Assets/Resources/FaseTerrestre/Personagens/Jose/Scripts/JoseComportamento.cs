using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class JoseComportamento : MonoBehaviour {

	public Transform myCollider;
	public Slider vida;
	private CameraFollow WorldCameraFollow;
	private JumpSystem meuJumpSystem;
	public Image armaHud;
	public Text textoMunicao;
	public Transform boneMaoDireita;
	public Transform boneMaoDireitaArma;
	public Transform boneDaArma;
	public float max_Descontrole;
	public float tempo_Descontrole;
	public float descontrole;

	private Rigidbody2D meuRigidbody2D;
	private Animator meuAnimator;

	private Vector2 velocidade; //Velocidade que vai ser usada para mover o personagem no meuRigidbody2D

	private bool recarregando;
	public bool possoPular = true;

	public float tempoAndar;
	public float tempoParar;

	public float forcaPulo;
	public float velo_Andando;
	public float velo_Correndo; //Velocidade a mais q vai ter quando estive correndo
	public float velo_Abaixado; //Velocidade a mais q vai ter quando estive correndo
	public float tempoOlhandoCima;
	private float olhandoCima;

	public float forcaDeslizar;
	private int direcaoDeslizar;

	public Transform[] lugarDasArmas;
	public Armas[] minhasArmas;
	public int armaAtiva = -1;

	public bool abaixado;
	private bool deslizando;
	private bool podePerderVida = true;

	public float maxTempoAtirando;
	private float tempoAtirando;

	//Chamado antes do Strat, mesmo se estre script estiver desativado
	void Awake () {
		WorldCameraFollow = FindObjectOfType<CameraFollow> ();
		meuJumpSystem = GetComponentInChildren<JumpSystem> ();
		meuAnimator = GetComponentInChildren <Animator> ();
		meuRigidbody2D = GetComponent<Rigidbody2D> ();

		for (int i = 0; i < minhasArmas.Length; i++) {
			minhasArmas [i].GetComponent<MudarPai> ().MudarMeuPai (lugarDasArmas[i]);
		}

		deslizando = false;
		recarregando = false;
		abaixado = false;
		possoPular = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (vida.value <= 0) {
			GetComponent<TrocarDeTela> ().CarregarCena ("GameOver");
		}

		if (armaAtiva >= 0) {
			Mirar ();
			Atirar ();

			textoMunicao.text = minhasArmas [armaAtiva].municoesNoPente.ToString () + "/" + minhasArmas [armaAtiva].municoesQuardada.ToString ();
		} else {
			textoMunicao.text = "0/0";
		}
		Movimentacao ();

		if (Input.GetKeyDown (KeyCode.Alpha1) && armaAtiva != 0) {
			TrocarDeArma (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && armaAtiva != 1) {
			TrocarDeArma (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && armaAtiva != 2) {
			TrocarDeArma (2);
		}

		AnimatorStateInfo temp_State = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (temp_State.IsTag ("Pulando") && !possoPular) {
			if (meuJumpSystem.isOnFloor) {
				possoPular = true;
				meuAnimator.SetTrigger ("TocouChao");
			}
		}
			
		if (Input.GetKeyDown (KeyCode.X) && possoPular && !deslizando && !temp_State.IsTag ("Deslizando") && !temp_State.IsTag ("DeslizandoAtirando")) {
			deslizando = true;
			StartCoroutine ("Deslizando");
			meuAnimator.SetTrigger ("Pular");
			possoPular = false;
			meuRigidbody2D.AddForce (transform.up * forcaPulo, ForceMode2D.Impulse);
		}

		if (temp_State.IsTag ("EmPe")) {
			myCollider.localScale = new Vector3 (myCollider.localScale.x, 1f, myCollider.localScale.z);
		} else if (temp_State.IsTag ("Abaixado")) {
			myCollider.localScale = new Vector3 (myCollider.localScale.x, 0.8f, myCollider.localScale.z);
		} else {
			myCollider.localScale = new Vector3 (myCollider.localScale.x, 0.6f, myCollider.localScale.z);
		}
		ImpedeDeSairDaCamera ();
	}

	IEnumerator Deslizando () {
		yield return new WaitForSeconds (2f);
		deslizando = false;
	}

	void FixedUpdate () {
		meuRigidbody2D.velocity = new Vector2 (velocidade.x, meuRigidbody2D.velocity.y);

		meuAnimator.SetFloat ("velocidade", Mathf.Abs (meuRigidbody2D.velocity.x));

		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (temp_AnimatorStateInfo.IsTag ("Deslizando")) {
			meuRigidbody2D.AddForce (transform.right * forcaDeslizar * direcaoDeslizar);
		}
	}

	private void ImpedeDeSairDaCamera () {
		float distanciaZ = (transform.position - Camera.main.transform.position).z;
		float bordaEsquerda = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanciaZ)).x;
		float bordadireita = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanciaZ)).x;

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, bordaEsquerda + 0.5f, bordadireita - 0.5f), transform.position.y, transform.position.z);
	}

	private void TrocarDeArma (int indice) {
		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (3);

		if (armaAtiva >= 0) {
			if (!temp_AnimatorStateInfo.IsTag ("Vazio") || recarregando || !minhasArmas [armaAtiva].TempoDeTiroPassou (abaixado)) {
				return;
			}

			minhasArmas [armaAtiva].enabled = false;
			minhasArmas [armaAtiva].GetComponent<MudarPai> ().MudarMeuPai (lugarDasArmas[armaAtiva]);
		}

		armaAtiva = indice;
		minhasArmas [armaAtiva].enabled = true;
		if (minhasArmas [armaAtiva].maoEsquerda) {
			minhasArmas [armaAtiva].GetComponent<MudarPai> ().MudarMeuPai (boneDaArma);
		} else {
			minhasArmas [armaAtiva].GetComponent<MudarPai> ().MudarMeuPai (boneMaoDireitaArma);
		}

		armaHud.sprite = minhasArmas [armaAtiva].armaHud;
		meuAnimator.SetInteger ("armaId", minhasArmas [armaAtiva].armaId);
		meuAnimator.SetTrigger ("trocarDeArma");
	}

	private void Mirar () {
		float temp_olhandoCima = 0;

		if (Input.GetKey (KeyCode.UpArrow)) {
			temp_olhandoCima = 45f;
		}

		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (1);
		if (meuAnimator.GetBool("deslizando") && temp_AnimatorStateInfo.IsTag("Parado") && Input.GetKey (KeyCode.DownArrow)) {
			temp_olhandoCima = -45f;
		}

		olhandoCima = Mathf.Lerp (olhandoCima, temp_olhandoCima, tempoOlhandoCima * Time.deltaTime);
		meuAnimator.SetFloat ("olhandoCima", olhandoCima);
	}

	private void Atirar () {
		tempoAtirando += Time.deltaTime; 

		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (3);

		if (minhasArmas [armaAtiva].tipoDeFogo == 0) {
			if (Input.GetKey (KeyCode.Z)) {
				tempoAtirando = 0;

				if (meuAnimator.GetFloat ("teclaCorrendo") < 1.1f &&
					temp_AnimatorStateInfo.IsTag ("Vazio") && !recarregando && minhasArmas [armaAtiva].PossoAtirar (abaixado)) {
					minhasArmas [armaAtiva].Atirar ();
					meuAnimator.SetTrigger ("atirar");
					meuAnimator.SetBool ("atirando", true);
				}
			} if (tempoAtirando > maxTempoAtirando || minhasArmas [armaAtiva].municoesNoPente == 0) {
				meuAnimator.SetBool ("atirando", false);

			}
		} else {
			if (Input.GetKeyDown (KeyCode.Z)) {
				tempoAtirando = 0;

				if (meuAnimator.GetFloat ("teclaCorrendo") < 1.1f &&
					temp_AnimatorStateInfo.IsTag ("Vazio") && !recarregando && minhasArmas [armaAtiva].PossoAtirar (abaixado)) {
					minhasArmas [armaAtiva].Atirar ();
					meuAnimator.SetTrigger ("atirar");
					meuAnimator.SetBool ("atirando", true);
				}
			} if (tempoAtirando > maxTempoAtirando || minhasArmas [armaAtiva].municoesNoPente == 0) {
				meuAnimator.SetBool ("atirando", false);
			}
		}

		if (Input.GetKeyDown (KeyCode.R) && minhasArmas [armaAtiva].PossoRecarregar () && minhasArmas [armaAtiva].TempoDeTiroPassou (abaixado) &&
			temp_AnimatorStateInfo.IsTag ("Vazio") && !recarregando) {

			StartCoroutine ("Recarregar");
			meuAnimator.SetTrigger ("recarregar");
			recarregando = true;
		}
	}

	private void Movimentacao () {
		int setaDirecao = 0;
		if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.LeftArrow)) {
			setaDirecao = 1;


		} else if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
			setaDirecao = -1;
		}
		
		//Faz o cano da arma vira de acordo com o eslacolamento do personagem
		if (armaAtiva >= 0) {
			if (transform.localScale.x > 0) {
				minhasArmas [armaAtiva].canoDaArma.localEulerAngles = new Vector3 (0f, 0f, 0f);
			} else {
				minhasArmas[armaAtiva].canoDaArma.localEulerAngles = new Vector3 (0f, 0f, 180f);
			}
		}

		AnimatorStateInfo temp_AnimatorStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (!temp_AnimatorStateInfo.IsTag ("Deslizando") && !temp_AnimatorStateInfo.IsTag ("DeslizandoAtirando")) {
			if (meuAnimator.GetBool ("abaixado") && setaDirecao != 0) {
				meuAnimator.SetFloat ("teclaCorrendo", 1);
				velocidade.Set (Mathf.Lerp (velocidade.x, (velo_Abaixado) * setaDirecao, tempoAndar * Time.deltaTime), velocidade.y);
				transform.localScale = new Vector3 (setaDirecao, 1, 1);
			} else {
				float teclaCorrendo = 1;
				if (Input.GetKey (KeyCode.LeftShift)) {
					teclaCorrendo = velo_Correndo;
				}
				meuAnimator.SetFloat ("teclaCorrendo", teclaCorrendo);

				if (setaDirecao == 0) {
					velocidade.Set (Mathf.Lerp (velocidade.x, 0, tempoParar * Time.deltaTime), velocidade.y);
				} else {
					velocidade.Set (Mathf.Lerp (velocidade.x, (velo_Andando + teclaCorrendo) * setaDirecao, tempoAndar * Time.deltaTime), velocidade.y);
					transform.localScale = new Vector3 (setaDirecao, 1, 1);
				}
			}
		} else {
			abaixado = true;
			meuAnimator.SetFloat ("teclaCorrendo", 1);
			velocidade.Set (Mathf.Lerp (velocidade.x, 0, tempoParar * Time.deltaTime), velocidade.y);
		}
		
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (meuAnimator.GetFloat ("velocidade") > 3.3f && possoPular) {
				meuAnimator.SetBool ("deslizando", true);
				meuAnimator.SetTrigger ("deslizar");

				direcaoDeslizar = setaDirecao;
			}

			abaixado = true;
			meuAnimator.SetBool ("abaixado", true);
		} else if (Input.GetKeyUp (KeyCode.DownArrow)) {
			abaixado = false;
			meuAnimator.SetBool ("abaixado", false);
		}

		if (temp_AnimatorStateInfo.IsTag ("Levantando")) {
			meuAnimator.SetBool ("deslizando", false);
		}
	}



	void OnTriggerEnter2D (Collider2D other) {
		//Ativa o ponto de spaw dos inimigos.
		if (other.tag == "PontoDeInimigos" && !other.GetComponent<PontoDeInimigos> ().enabled) {
			WorldCameraFollow.target = other.transform;
			other.GetComponent<Collider2D> ().enabled = false;
			other.GetComponent<PontoDeInimigos> ().enabled = true;
			other.transform.position = new Vector2 (other.transform.position.x, transform.position.y);
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Espinho") {
			PerderVida (4);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.transform.tag == "Vida") {
			Destroy (other.gameObject);
			vida.value += 10;
		} else if (other.transform.tag == "Ammo") {
			if (armaAtiva > 0) {
				
				if (!AdicionarMuncao (armaAtiva)) {
					if (armaAtiva == 1) {
						AdicionarMuncao (2);
					} else {
						AdicionarMuncao (1);
					}
				}
			}

			Destroy (other.gameObject);
		} if (other.transform.tag == "GameOver") {
			vida.value = 0;
		}  if (other.transform.tag == "Venceu") {
			GetComponent<TrocarDeTela> ().CarregarCena ("Venceu");
		}
	}

	private bool AdicionarMuncao (int indice) {
		if (minhasArmas [indice].municoesQuardada < minhasArmas [indice].maxMunicao * 8) {
			minhasArmas [indice].municoesQuardada += Random.Range (minhasArmas [indice].maxMunicao / 2, minhasArmas [indice].maxMunicao * 2);

			if (minhasArmas [indice].municoesQuardada > minhasArmas [indice].maxMunicao * 8) {
				minhasArmas [indice].municoesQuardada = minhasArmas [indice].maxMunicao * 8;
			}

			return true;
		} else {
			return false;
		}
	}

	IEnumerator Recarregar () {
		yield return new WaitForSeconds (2f);
		recarregando = false;
	}

	public void PerderVida (float dano) {
		if (podePerderVida) {
			meuAnimator.SetTrigger ("levarDanoFrente");
			vida.value -= dano;
			podePerderVida = false;
			StartCoroutine ("PodePerderVida");
		}
	}

	IEnumerator PodePerderVida () {
		yield return new WaitForSeconds (1.5f);
		podePerderVida = true;
	}

	public void PorMunicao () {
		minhasArmas [armaAtiva].Recarregar ();
	}

	public void DroparCarregador () {
		minhasArmas [armaAtiva].DroparCarregador (boneMaoDireita);
	}

	public void PuxarFerrolho () {
		minhasArmas [armaAtiva].PuxarFerrolho ();
	}

	public void EmpurarFerrolho () {
		minhasArmas [armaAtiva].EmpurarFerolho ();
	}
}
