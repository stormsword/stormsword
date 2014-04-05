using UnityEngine;
using System.Collections;

public class mouseButton : MonoBehaviour {
	void OnMouseDown() {
		// Check to see if mouse is pressed down
		Debug.Log("Mouse is down on " + this.name + "!");
	}

	void OnMouseUp() {
		// Check to see if mouse is pressed down
		Debug.Log("Mouse is up on " + this.name + "!");
	}

}