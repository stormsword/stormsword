using UnityEngine;
using System.Collections;

/* moveScript - Moves the current game object */

public class MoveScript : MonoBehaviour {

	// Components
	private Animator animator;

	// Speed of object
	public float speed = 400;

	// Direction of object
	public Vector2 direction = new Vector2(-1, 0);
	public Vector2 facing = new Vector2(-1, 0);

	// Actual movement
	private Vector2 movement = new Vector2(0, 0);
	private bool isMoving = false;
	
	void Awake () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		/* Play idle animation */
		if(animator)
		{
		animator.SetFloat ("facing_x", facing.x);
		animator.SetFloat ("facing_y", facing.y);

		/* Check if character is moving */

		if(rigidbody2D.velocity.normalized.x != 0 || rigidbody2D.velocity.normalized.y != 0) {
			// Store the direction the player is facing in case they stop moving
			facing = rigidbody2D.velocity.normalized;
			isMoving = true;
		}

		movement = direction * speed;	// Calculate movement amount




		/* Play walking animation */

		if (isMoving == true) {

			animator.SetBool ("isMoving", isMoving);
			animator.SetFloat ("movement_x", movement.x);
			animator.SetFloat ("movement_y", movement.y);
		}
		}

	
}


	void FixedUpdate() {
		// Apply the movement to the rigidbody
		rigidbody2D.AddForce (movement);

		// Rotate rigidbody accordingly
	}

	/* Moves the object in a direction 
	 	float input_X: Input for X-axis movement (b/t -1 and 1)
	 	float input_Y: Input for Y-axis movement (b/t -1 and 1)
	 */
	internal void Move(float input_X, float input_Y) {
		direction = new Vector2(input_X, input_Y);
	}
}
