using UnityEngine;
using System.Collections;

/* menuScript - Display a menu when the player pauses. Hide the menu when unpaused */

public class PauseMenuScript : MonoBehaviour {

	public static PauseMenuScript instance;

	public Rect unpauseButtonPosition;
	public string unpauseText;

	public Rect quitButtonPosition;
	public string quitText;

	private CharacterScript characterScript;

	void Start() {
		instance = this;
		this.enabled = false;	// The game can enable this script when the game is paused
		GameObject player = GameObject.FindWithTag("Player");
		characterScript = player.GetComponent<CharacterScript>();
	}

	// We don't show anything until the script is enabled
	void OnGUI() {
		// Draw black background
		GUI.backgroundColor = Color.black;
		GUI.color = Color.white;
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Pause");	// Draw a box that covers the whole screen

		// Draw buttons
		var unpauseButton = GUI.Button(unpauseButtonPosition, unpauseText);
		var quitButton = GUI.Button(quitButtonPosition, quitText);

		// Check for button clicks
		if(unpauseButton) {
			characterScript.Pause();
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
	}
}
