using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarPai : MonoBehaviour {
	
	public void MudarMeuPai (Transform pai) {
		transform.SetParent (pai);

		if (pai != null) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}

	public void MudarMeuPai (Transform pai, Animator animator, string parametro, bool zerarPosicao) {
		//transform.SetParent (pai);

		animator.SetInteger (parametro, animator.GetInteger (parametro) + 1);
	}
}
