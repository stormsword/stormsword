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
	
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate movement direction
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);

		
		/* Play movement animation */
		// 0 - Up, 1 - Right, 2 - Down, 3 - Left
		// Player moving right
		if(movement.x > 0) {
			animator.SetInteger("direction", 1);
			animator.SetFloat("speed", speed.x);
		}
		else if(movement.x < 0) {
			animator.SetInteger("direction", 3);
			animator.SetFloat("speed", speed.x);
		}
		else if(movement.y > 0) {
			animator.SetInteger("direction", 0);
			animator.SetFloat("speed", speed.y);
		}
		else if(movement.y < 0) {
			animator.SetInteger("direction", 2);
			animator.SetFloat("speed", speed.y);
		}
	}

	void FixedUpdate() {
		// Apply the movement to the rigidbody
		rigidbody2D.velocity = movement;
	}
}
