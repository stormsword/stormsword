using UnityEngine;
using System.Collections;

public class RockSlideScript : MonoBehaviour {


	public Transform RockSlide;

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter2D(Collider2D defender) {
		// Make sure it's a player that triggers the collision
		if(defender.tag == "Player") {

			Debug.Log("Player entered rockslide area!");
			// Spawn Rockslide object
			GameObject rockslide = Instantiate(RockSlide) as GameObject;

			// When boss is dead, destroy it
		}
	}

	void SpawnRockslide() {

	}
}
