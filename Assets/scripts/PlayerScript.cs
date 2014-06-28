using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Speed of the player
	public Vector2 speed = new Vector2(50, 50);

	// Store the player's movement
	private Vector2 movement;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Retrieve axis information from controller
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Calculate movement per-direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
	}

	// Physics calcs
	void FixedUpdate() {
		// Move the game object
		rigidbody2D.velocity = movement;
	}
}
