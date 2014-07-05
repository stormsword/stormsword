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
		/* Check if character is moving */
		movement = direction * speed;	// Calculate movement amount


		if(movement.x != 0 || movement.y != 0) {
			facing = new Vector2(Mathf.CeilToInt(direction.x), Mathf.CeilToInt (direction.y));	// Round direction up because direction.x/y can be 0.2 or 0.4, etc.
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
		rigidbody2D.AddForce (movement);

		// Rotate rigidbody accordingly
	}
}
