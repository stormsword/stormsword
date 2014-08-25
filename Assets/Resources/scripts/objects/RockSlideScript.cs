using UnityEngine;
using System.Collections;

public class RockSlideScript : MonoBehaviour {

	[Tooltip("The Game Object to spawn when the player enters the trigger")]
	public Transform RockSlide;

	[Tooltip("Should this only happen once? Or many times?")]
	public bool onlyOnce = true;

	private BoxCollider2D trigger;

	// Use this for initialization
	void Start () {
		trigger = GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D defender) {
		// Make sure it's a player that triggers the collision
		if(defender.tag == "Player") {
			// Spawn Rockslide object
			GameObject rockslide = Instantiate(RockSlide) as GameObject;

			if(onlyOnce) {
				// Disable this trigger because the rockslide should only trigger once
				trigger.enabled = false;
			}
		}
	}
}
