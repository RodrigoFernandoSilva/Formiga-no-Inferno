using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawFormiga : MonoBehaviour {

	public GameObject soldadaFrefab; //Objeto a ser criado, tem q ser um prefab
	public float xMinimo = -10f; //distancia minima a ser sorteada
	public float xMaximo = 10f; //distancia minima a ser sorteada
	public float yPosicao = 5f; //altura a ser sorteada

	void CriarFormiga () {
		Vector2 posicao = new Vector2 (Random.Range (xMinimo, xMaximo), yPosicao);
		//Instantiate é o método q cria objetos na tela, tem varias formas de usar ele, uma delas vc cria o objeto filho de alguem, a outra q foi a q usei, vc cria
		//o objeto em uma posição sejeada, poderia ser a posição de um outro objeto
		Instantiate (soldadaFrefab, posicao, Quaternion.identity); //Quaternion.identity diz q a rotação do objeto vai ser 0
	}
}
