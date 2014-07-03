using UnityEngine;
using System.Collections;

/* EnemyScript - Generic Enemy Behavior */

public class EnemyScript : MonoBehaviour {

	private CharacterScript characterScript;

	public string characterType = "Enemy";

	void Awake() {
		characterScript = GetComponent<CharacterScript>();
	}

	void Update() {
		// Continuously attack until dead
		characterScript.Attack();
	}
}
