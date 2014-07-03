using UnityEngine;
using System.Collections;

/* EnemyScript - Generic Enemy Behavior */

public class EnemyScript : MonoBehaviour {

	private CharacterScript characterScript;

	void Awake() {
		characterScript = GetComponent<CharacterScript>();
	}

	void Update() {
		// Continuously attack until dead
		characterScript.Attack();
	}
}
