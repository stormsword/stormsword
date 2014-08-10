using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// Needed for IDictionary to be used in types

/* TrackingScript - Tracks basic usage data for bugfixing/reporting purposes
 * 
 * This document will ALWAYS be updated with what we track.
 * 
 * What we DO NOT track:
 * -Anything outside of the game
 * -Your login, password, username, etc.
 * -Your location
 * -Anything that could identify you
 * 
 * What we track:
 * -Version of your OS
 * -Your screen resolution
 * -What scene has recently loaded
 * 
 * For more info on the mixpanel API: https://github.com/waltdestler/Mixpanel-Unity-CSharp
 */

public class TrackingScript : MonoBehaviour {

	private string mixpanelKey;	// Should be configured externally via environment variables

	public static TrackingScript instance;	// Only one trackinscript will exist within the game at any time so it can be available everywhere

	// Use this for initialization
	void Awake () {
		instance = this;

		// Check if debug build or release build
		if(Debug.isDebugBuild) {
			// Debug.isDebugBuild will always be set true in Unity's Editor as well as when 'Development Build' is checked
			mixpanelKey = "b5b6ee0e037c74e5e6563d56b71acf86";	// Development Key
		}
		else {
			mixpanelKey = "cc86ad2f0f27fb4334e77933e22a119c";	// Production Key
		}

		Mixpanel.Token = mixpanelKey;

		// Set 'supertracking' variables
		AddProperty("platform", Application.platform.ToString());
		AddProperty("resolution", Screen.width + "x" + Screen.height);
	}

	/* Track - Sends an event to mixpanel for tracking
	 * string key - The key of the object you want to track. Should be the name of an event with underscores for spaces */
	public void Track(string key) {
		Mixpanel.SendEvent(key);
	}
	/* Track - Sends an event to mixpanel for tracking
	 * string key - The key of the object you want to track. Should be the name of an event with underscores for spaces
	 * IDictionary value - The data you want to send for this event. Can be any type of object */
	public void Track(string key, IDictionary<string, object> properties) {
		Mixpanel.SendEvent(key, properties);
	}

	/* AddProperty - Add a SuperProperty to Mixpanel */
	public void AddProperty(string key, object value) {
		Mixpanel.SuperProperties.Add (key, value);
	}

	// StopTracking - Used if the player chooses to disable Mixpanel
	public void StopTracking() {
		this.enabled = false;
	}
}
