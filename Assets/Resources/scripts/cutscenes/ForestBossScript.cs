using UnityEngine;
using System.Collections;

/* ForestBossScript - Scripted event kicked off by a trigger
 * 
 * Pauses the action and plays a cutscene:
 * Pan to boss waiting on a dais
 * Rocks fall behind you trapping you in the room when you enter the room
 * Pause player movement and pan camera to boss
 * Return camera to player and rockslide triggers
 * Boss jumps into the center when you enter the room
*/
public class ForestBossScript : MonoBehaviour {

	[Tooltip("Should this only happen once? Or many times?")]
	public bool onlyOnce = true;

	// Trigger that kicks off the whole shabang
	private BoxCollider2D trigger;


	// Player info
	private GameObject player;
	private PlayerScript playerScript;

	// Camera info
	private GameObject mainCamera;
	private CameraScript cameraScript;

	// Boss info
	private GameObject boss;
	
	// Start the rock slide
	private RockSlideScript rockSlideScript;

	void Start () {
		// Get trigger so we can disable it after it happens
		trigger = GetComponent<BoxCollider2D>();

		// Grab player info
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<PlayerScript>();

		// Grab camera info
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		cameraScript = mainCamera.GetComponent<CameraScript>();

		// Grab boss info
		boss = GameObject.Find ("Enemies_Dabossman");

		// Get rockslide info
		rockSlideScript = GetComponent<RockSlideScript>();
	}

	void OnTriggerEnter2D(Collider2D defender) {
		// Make sure it's a player that triggers the collision
		if(defender.tag == "Player") {
			// Spawn Rockslide object
			if(onlyOnce) {
				StartCoroutine(StartEvent());	// Kick off the event

				trigger.enabled = false; // Disable this trigger because the event should only trigger once
			}
		}
	}

	// StartEvent - Kicks off the scripted event (usually after a trigger or state change)
	IEnumerator StartEvent() {

		// Lock player input
		playerScript.ToggleInput();

		// Camera - Change camera mode (move to position)
		cameraScript.Goto(boss.transform.position);

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
		yield return new WaitForSeconds(2);	// For some reason when I add this - the function never runs.
		// yield Camera -> Pan back to player
		// Camera - change camera mode (follow player)
		// Unlock Player input
		playerScript.ToggleInput();
	}
}
