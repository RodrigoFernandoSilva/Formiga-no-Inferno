using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigaAnimator : MonoBehaviour {

	private InimigaIA fatherInimigaIA;

	void Awake () {
		fatherInimigaIA = GetComponentInParent <InimigaIA> ();
	}

	private void DroparCarregador () {
		fatherInimigaIA.DroparCarregador ();
	}


	private void PorCarregador () {
		fatherInimigaIA.PorMunicao ();
	}

	private void PuxarFerrolho () {
		fatherInimigaIA.PuxarFerrolho ();
	}

	private void EmpurarFerrolho () {
		fatherInimigaIA.EmpurarFerrolho ();
	}
}
