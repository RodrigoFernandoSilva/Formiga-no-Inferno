using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municoes : MonoBehaviour {

	private Rigidbody2D meuRigidbody2D;

	public float dano;
	public float velocidade;

	void Start () {
		meuRigidbody2D = GetComponent<Rigidbody2D> ();

		meuRigidbody2D.velocity = transform.right * velocidade;

		if (transform.tag == "i_municao") {
			GetComponent<Municoes> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float distanciaZ = (transform.position - Camera.main.transform.position).z;
		float bordaEsquerda = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanciaZ)).x;
		float bordadireita = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanciaZ)).x;

		if (transform.position.x < bordaEsquerda - 0.5f || transform.position.x > bordadireita + 0.5f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		bool destroir = false;

		if (transform.tag == "p_municao") {
			if (transform.tag != "Player") {
				destroir = true;
			}

			if (other.transform.tag == "Inimiga") {
				other.transform.GetComponent<InimigaIA> ().PerderVida (dano);
			}
		} else if (transform.tag == "i_municao") {
			if (transform.tag != "Inimiga") {
				destroir = true;
			}

			if (other.transform.tag == "Player") {
				other.transform.GetComponent<JoseComportamento> ().PerderVida (dano / 3f);
			}
		}

		if (destroir) {
			Destroy (gameObject);
		}
	}
}
