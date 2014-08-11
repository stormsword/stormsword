using UnityEngine;
using System.Collections;

/* OptionsScript - Allows the player to configure any options they may have set */
public class OptionsScript : MonoBehaviour {
	
	PauseMenuScript pauseMenu; // Need to include this so we can show the options menu again after the player exits the menu.
	
	public Rect disableTrackingPosition;
	public string disableTrackingText;

	// Return to previous menu button
	public Rect previousMenuPosition;
	public string previousMenuText;

	// Use this for initialization
	void Start () {
		this.enabled = false;

		pauseMenu = transform.parent.GetComponentInChildren<PauseMenuScript>();
	}

	void OnGUI() {
		GUI.backgroundColor = Color.black;
		GUI.color = Color.white;

		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Options");	// Draw a box that covers the whole screen

		// Draw buttons
		bool disableTrackingButton = GUI.Button(disableTrackingPosition, disableTrackingText);
		bool previousMenuButton = GUI.Button(previousMenuPosition, previousMenuText);

		// Check for button clicks
		if(disableTrackingButton) {
			DisableTracking();
		}
		if(previousMenuButton) {
			HideOptions();
		}
	}

	/* ShowOptions - Displays the options menu */
	public void ShowOptions() {
		this.enabled = true;	// Draw the interface (via OnGUI)
	}

	/* HideOptions - Used to close the options menu and show the previous menu */
	void HideOptions() {
		this.enabled = false;
		pauseMenu.enabled = true;
	}

	/* DisableTracking - Disables tracking via mixpanel */
	void DisableTracking() {
		TrackingScript.instance.StopTracking();
	}

}
