using UnityEngine;
using System.Collections;

/* moveScript - Moves the current game object */

public class MoveScript : MonoBehaviour {

	// Components
	private Animator animator;

	// Speed of object
	public Vector2 speed = new Vector2(10, 10);

	// Direction of object
	public Vector2 direction = new Vector2(-1, 0);

	// Actual movement
	private Vector2 movement;
	private bool isMoving = false;
	
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate movement direction
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);

		/* Check if character is moving */
		if(movement.x != 0 || movement.y != 0) {
			isMoving = true;
		}
		else {
			isMoving = false;
		}

		/* Play movement animation */
		animator.SetBool ("isMoving", isMoving);
		animator.SetFloat("movement_x", movement.x);
		animator.SetFloat("movement_y", movement.y);
	}

	void FixedUpdate() {
		// Apply the movement to the rigidbody
		rigidbody2D.velocity = movement;
	}
}
