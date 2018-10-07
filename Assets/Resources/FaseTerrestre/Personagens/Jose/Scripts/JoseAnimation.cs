using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoseAnimation : MonoBehaviour {
	
	private JoseComportamento fatherJoseComportamento;

	void Awake () {
		fatherJoseComportamento = GetComponentInParent <JoseComportamento> ();
	}

	private void DroparCarregador () {
		fatherJoseComportamento.DroparCarregador ();
	}


	private void PorCarregador () {
		fatherJoseComportamento.PorMunicao ();
	}

	private void PuxarFerrolho () {
		fatherJoseComportamento.PuxarFerrolho ();
	}

	private void EmpurarFerrolho () {
		fatherJoseComportamento.EmpurarFerrolho ();
	}
}
