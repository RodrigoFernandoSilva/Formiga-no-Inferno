using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystem : MonoBehaviour {

	public bool isOnFloor;

	void OnTriggerStay2D(Collider2D col) {
		isOnFloor = true;
	}

	void OnTriggerExit2D(Collider2D col) {
		isOnFloor = false;
	}
}
