using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirMouse : MonoBehaviour {

	public Camera cameraPrincipal; //Camera pricipal q vai ser usada como referencia para a posição do mouse na tela
	private Vector3 novaPosicaoDaFormiga; //Quarda a nova posição da formiga, podia ser colocado tireto no transforma, mais seprado fica mais facil de entender
	private Vector3 posicaoDoMouse = Vector3.zero; //Posição do mouse na tela

	//Quando o objeto e clicado e arastado, esse método é chamado automaticamente pela propia unity "é tipo um update, só q pra drag do mouse"
	void OnMouseDrag () {
		//Guarda a posição do mouse na tela, porém a posição em <z> q seria a profundidade tem q ser mantida igual, pq se não a formiga entre dentro da câmera
		//Então como a formiga esta na posição 0, só é colocar posição da camare em z, para meio q dizer q o mouse esta a frente da camera, só q tem q ser a posição
		//em <z> positica, por isso q to passando a posição <z> da camera negativa
		posicaoDoMouse.Set (Input.mousePosition.x, Input.mousePosition.y, -cameraPrincipal.transform.position.z);

		//Agora uso um método da propia unity para saber onde é q o mouse esta em relação ao mundo 3d do jogo e guardo essa posição em uma variavel
		novaPosicaoDaFormiga = cameraPrincipal.ScreenToWorldPoint (posicaoDoMouse);

		//Após tudo isso eu passo a posição da variavel para a formiga, como é um vector 3, tem a posições (x, y, z)
		transform.position = novaPosicaoDaFormiga;

		//----- GetComponent<Nome Do Codigo D aFormiga> ().enabled = false; -----//
	}

	void OnMouseUp () {
		//----- GetComponent<Nome Do Codigo D aFormiga> ().enabled = true; -----//
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.transform.tag == "Morte") {
			Destroy (gameObject);
		}
	}
}