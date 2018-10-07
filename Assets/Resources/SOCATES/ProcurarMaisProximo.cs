using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcurarMaisProximo : MonoBehaviour {

	private Rigidbody2D meuRigidbody2D;
	private Transform inimigaMaisProxima;

	private bool chegouNaInimiga;

	public float velocidaeX;
	public float distanciaParaVer;
	public float distanciaDeAtaque;

	private int direcao;


	//Essa função é chamada antes do Start, e diferente do Start, mesmo se este código não estiver ativado, o Awake é chamado pela pripia Unity
	void Awake () {
		meuRigidbody2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Se a soldada não tiver nenhuma formiga para atacar, é procurado uma até achar
		if (inimigaMaisProxima == null) {
			ProcurarInimigaMaisProxima ();
		}
		
		if (inimigaMaisProxima != null) {
			//Caso a distancia entre os objetos seja menor a q distancia minima de ataque, a formiga soldada itra parar de de mecher
			if (Vector2.Distance (transform.position, inimigaMaisProxima.position) < distanciaDeAtaque) {
				chegouNaInimiga = true;
			} else {
				chegouNaInimiga = false;
			}

			//Faz a formiga se mover en direção a inimiga, para isso preciso saber se sua posição é maior ou menor do que a formiga soldada
			if (transform.position.x > inimigaMaisProxima.position.x) {
				direcao = -1;
			} else {
				direcao = 1;
			}
		}
	}

	void FixedUpdate () {
		//A formiga só pode se mover caso tenha alguma inimiga no cenario e caso ela ainda não tenha chegado na menor distancia de ataque
		if (inimigaMaisProxima != null && !chegouNaInimiga) {
			meuRigidbody2D.velocity = new Vector2 (velocidaeX * direcao, meuRigidbody2D.velocity.y);
		}
	}

	private void ProcurarInimigaMaisProxima () {
		/*
		 * Procura todos os objetos com a tag "Ïnimiga" em todo o cenario, na verdade todos os metodos de procurar algum objeto ou pegar algum compon.ente tem
		 * Como pegar varios, como por exemplo o "GetComponents", mais ele tem q ser um vetor pq ele vai receber os elementos retornados do método
		 */
		GameObject[] todasInimigas = GameObject.FindGameObjectsWithTag ("Inimiga");

		//Caso esse vetor seja igual a 0, quer dizer q não tem nenhuma formiga por perto, por isso eu aborto esse método.
		if (todasInimigas.Length == 0) {
			//Para abortar basta dar um "return" sem nenhum elemento de tetorno pois esse método é void, da pra fazer isso em todos os metodos, ate no update e taus.
			return;
		}

		//Agora eu preciso encontra o objeto com a menor distancia, para isso eu primeiro assumo q a primeira posição do vetor é a menor, então vou rodando
		//o vetor todo até achar um q tenha a menor distancia.
		inimigaMaisProxima = todasInimigas[0].transform;
		float menorDistancia = Vector2.Distance (transform.position, inimigaMaisProxima.position);

		float distanciaTemporaria;
		for (int linha = 0; linha < todasInimigas.Length; linha++) {
			distanciaTemporaria = Vector2.Distance (transform.position, todasInimigas [linha].transform.position);

			if (menorDistancia >  distanciaTemporaria) {
				inimigaMaisProxima = todasInimigas [linha].transform;
				menorDistancia = distanciaTemporaria;
			}
		}

		//Caso a distancia dessa formiga mais proxima encontrada for maior q a distancia que a soldada pode emxergar, a formiga mais proxima e deixada
		//como nulo, pois assim a soldada não seguira ninguem
		if (menorDistancia > distanciaParaVer) {
			inimigaMaisProxima = null;
		}
	}
}
