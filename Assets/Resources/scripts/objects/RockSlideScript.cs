using UnityEngine;
using System.Collections;

public class RockSlideScript : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
//		player = GameObject.FindGameObjectWithTag('Player');		
	}

	void OnTriggerEnter2D(Collider2D defenderCollider) {
		// Spawn Rockslide object
		// Make usre player can't attakc it
		// When boss is dead, destroy it	
	}
}
