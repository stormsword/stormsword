using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* ScrollingScript - Used to scroll a background or static object 

 Usage: Assign to a layer you want to scroll

 isLinkedToCamera (bool) - Does the current object also move the camera? */

public class ScrollingScript : MonoBehaviour {

	public Vector2 speed = new Vector2(2, 2);
	public Vector2 direction = new Vector2(-1, 0);

	public bool isLinkedToCamera = false;

	void Start() {

	}

	// Update is called once per frame
	void Update () {

		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

		// Move the Camera
		if(isLinkedToCamera) {
			Camera.main.transform.Translate(movement);
		}
	}
}
