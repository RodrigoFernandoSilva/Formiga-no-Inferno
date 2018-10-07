using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBoxCollider2D : MonoBehaviour {

	public int boxAhSerDesativado;
	public Collider2D[] meusBoxCollider2D; //Guarda todos os <boxCollider2D> do objeto q receber esse codigo

	// Use this for initialization
	void Start () {
		meusBoxCollider2D = GetComponents<Collider2D> (); //Carrega os <boxCollider2D> para a variavel

		DesatovarBoxColider (boxAhSerDesativado);
	}

	/// <summary>
	/// Desatica o box colider da variavel <meusBoxCollider2D>, o indice não pode passar do tamanho do vetor
	/// e o vetor começa no indice 0, só lembrando
	/// </summary>
	/// <param name="indice">Indice.</param>
	private void DesatovarBoxColider (int indice) {
		meusBoxCollider2D [boxAhSerDesativado].enabled = false;
	}
}
