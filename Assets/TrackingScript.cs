using UnityEngine;
using System.Collections;

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
 * -Version of the game you're running
 * -Which character you're playing
 * -What scene you're on
 */

public class TrackingScript : MonoBehaviour {

	private string MIXPANEL_KEY;	// Should be configured externally via environment variables

	public static TrackingScript instance;	// Only one trackinscript will exist within the game at any time so it can be available everywhere

	// Use this for initialization
	void Start () {
		instance = this;

		// Check if debug build or release build
		// Set key appropriately
		// Initialize mixpanel key

		// Set 'supertracking' variables
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
