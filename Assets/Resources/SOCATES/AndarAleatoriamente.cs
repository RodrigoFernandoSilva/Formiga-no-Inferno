using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndarAleatoriamente : MonoBehaviour {

	//Variaveis do seu propio código
	private Rigidbody2D meuRigidbody2D;
	public float velocidade;



	private bool podeMover = false;
	private int direcao; //Direção do movento e dada em -1 e 1;
	private float tempoMovendo; //Tempo q a soldada vai ficar movento na direção q foi sorteda
	public Vector2 tempoMovendoRange; //Range do tampo a ser sorteado;
	public Vector2 campoDeAcao; //Campo de movimetação da formiga, se ela sair fora dos limites ela começa a andar pro outro lado
	private float contador; //Contador q vai de 0 até o tempo sorteado
	private float tempoSorteado; //Tempo que a formiga vai ficar parada até poder começara se mover
	public Vector2 tempoRange; //Range do tempoa ser sorteado.

	// Use this for initialization
	void Start () {
		meuRigidbody2D = GetComponent<Rigidbody2D> ();

		SortearValores ();
	}

	void FixedUpdate () {
		MoverDuranteTempo ();
	}

	/// <summary>
	/// Faz a soldada se mover sosinha.
	/// </summary>
	private void MoverDuranteTempo() {
		if (podeMover) {
			meuRigidbody2D.velocity = new Vector2 (velocidade * direcao, meuRigidbody2D.velocity.y);

			if (transform.position.x > campoDeAcao.y) {
				direcao = -1;
			} else if (transform.position.x < campoDeAcao.x) {
				direcao = 1;
			}

			tempoMovendo -= Time.deltaTime;

			if (tempoMovendo < 0) {
				podeMover = false;
				SortearValores ();
			}
		} else {
			EsperarTempoSorteado ();
		}
	}

	/// <summary>
	/// Método q espera o tempo da formiga ficar parada acabar.
	/// </summary>
	void EsperarTempoSorteado () {
		contador += Time.deltaTime;

		if (contador > tempoSorteado) {
			//Depois q o tempo acaba, é sorteado novos dados e a formiga começa a se move sosinha.
			podeMover = true;
		}
	}

	/// <summary>
	/// Sorteia dados aleatorio
	/// </summary>
	void SortearValores () {
		contador = 0;

		tempoMovendo = Random.Range (tempoMovendoRange.x, tempoMovendoRange.y);
		tempoSorteado = Random.Range (tempoRange.x, tempoRange.y);

		//Sorteia a direção q o personagem vai se mover, o random vai de 0 a 1, então é 50% de chance pra cada
		if (Random.Range (0, 2) == 0) {
			direcao = -1;
		} else {
			direcao = 1;
		}
	}
}
