using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	private Transform jose;
	public Vector2 smoothSleep;
	private Vector2 position;
	public Vector2 centerDistance;

	public Transform limitador;

	public float smooth = 0.5f;

	//640 = -15.55
	//n   =  x?

	void Awake () {
		jose = FindObjectOfType<JoseComportamento> ().transform;

		position.Set (transform.position.x, transform.position.y);

		target = jose;
	}

	// Update is called once per frame
	void Update () {
		if (jose.position.x < limitador.position.x) {
			limitador.position = new Vector2 (limitador.position.x, jose.position.y);
			target = limitador;
		} if (target == limitador && jose.position.x > limitador.position.x) {
			target = jose;
		}

		if (target == jose) {
			if (target.localScale.x > 0) {
				position.x = Mathf.SmoothDamp (position.x, target.position.x + centerDistance.x, ref smoothSleep.x, smooth);
			} else {
				position.x = Mathf.SmoothDamp (position.x, target.position.x - centerDistance.x, ref smoothSleep.x, smooth);
			}
		} else {
			if (target == null) {
				target = jose;
			} else {
				position.x = Mathf.SmoothDamp (position.x, target.position.x, ref smoothSleep.x, smooth);
			}
		}

		position.y = Mathf.SmoothDamp (position.y, target.position.y + centerDistance.y, ref smoothSleep.y, smooth);

		transform.position = new Vector3 (position.x, position.y, transform.position.z);
	}
}
