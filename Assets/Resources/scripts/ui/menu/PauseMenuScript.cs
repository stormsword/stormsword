using UnityEngine;
using System.Collections;

/* menuScript - Display a menu when the player pauses. Hide the menu when unpaused */

public class PauseMenuScript : MonoBehaviour {

	public static PauseMenuScript instance;

	public Rect unpauseButtonPosition;
	public string unpauseText;

	public Rect optionsButtonPosition;
	public string optionsText;

	public Rect quitButtonPosition;
	public string quitText;

	private CharacterScript characterScript;

	private OptionsScript optionsScript;

	void Start() {
		instance = this;
		this.enabled = false;	// The game can enable this script when the game is paused
		GameObject player = GameObject.FindWithTag("Player");
		characterScript = player.GetComponent<CharacterScript>();
		optionsScript = transform.parent.GetComponentInChildren<OptionsScript>();								
	}

	// We don't show anything until the script is enabled
	void OnGUI() {
		// Draw black background
		GUI.backgroundColor = Color.black;
		GUI.color = Color.white;
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Pause");	// Draw a box that covers the whole screen

		// Draw buttons
		var unpauseButton = GUI.Button(unpauseButtonPosition, unpauseText);
		var optionsButton = GUI.Button (optionsButtonPosition, optionsText);
		var quitButton = GUI.Button(quitButtonPosition, quitText);

		// Check for button clicks
		if(unpauseButton) {
			characterScript.Pause();
		}
		if(optionsButton) {
			this.enabled = false;	// Hide current menu
			optionsScript.ShowOptions();	// Pass in gameObject so the current menu can be re-activated when the user exists the options menu
		}
		if(quitButton) {
			characterScript.Quit();
		}
	}

	/* Pauses the game and displays a menu */
	public void Pause() {
		this.enabled = true;	// Start showing stuffs
	}

	/* Unpauses the game and hides the menu */
	public void UnPause() {
		this.enabled = false;	// Stop showing stuffs
		if(optionsScript.enabled) {
			optionsScript.enabled = false;	// Close options menu if it's open and the player unpauses
		}
	}
}
