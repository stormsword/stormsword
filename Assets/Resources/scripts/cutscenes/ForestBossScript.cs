using UnityEngine;
using System.Collections;

/* ForestBossScript - Pauses the action and plays a cutscene:
 * Pan to boss waiting on a dais
 * Rocks fall behind you trapping you in the room when you enter the room
 * Pause player movement and pan camera to boss
 * Return camera to player and rockslide triggers
 * Boss jumps into the center when you enter the room
*/
public class ForestBossScript : MonoBehaviour {

	[Tooltip("Should this only happen once? Or many times?")]
	public bool onlyOnce = true;

	// Player info
	private GameObject player;
	private PlayerScript playerScript;

	// Trigger that kicks off the whole shabang
	private BoxCollider2D trigger;

	// Start the rock slide
	private RockSlideScript rockSlideScript;

	void Start () {
		// Get trigger so we can disable it after it happens
		trigger = GetComponent<BoxCollider2D>();

		// Grab player info
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<PlayerScript>();

		// Get rockslide info
		rockSlideScript = GetComponent<RockSlideScript>();
	}

	// StartEvent - Kicks off the scripted event (usually after a trigger or state change)
	void StartEvent() {
		// Lock player input
		playerScript.ToggleInput();

		// Find Boss
		// Find Camera
		// Camera - Change camera mode (move to position)
		// yield Camera -> Goto X/Y (boss)
		// WaitForSeconds(5)
		// yield Camera -> goto x/y (player)
		
		// Drop stones behind player
		rockSlideScript.SpawnSlide();
		// Player looks around (confused animation)
		// WaitForSeconds(3)
		// yield Camera -> goto middle of room
		// Boss jumps down + charges at player
		// WaitForSeconds(3)
		// yield Camera -> Pan back to player
		// Camera - change camera mode (follow player)
		// Unlock Player input
	}
	
	void OnTriggerEnter2D(Collider2D defender) {
		// Make sure it's a player that triggers the collision
		if(defender.tag == "Player") {
			// Spawn Rockslide object
			if(onlyOnce) {
				// Disable this trigger because the rockslide should only trigger once
				trigger.enabled = false;
				StartEvent();
			}
		}
	}
}
