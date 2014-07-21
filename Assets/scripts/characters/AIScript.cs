using UnityEngine;
using System.Collections;

/* AIScript moves the enemy at random */

public class AIScript : MonoBehaviour {

	/* Components */
	private Animator animator;
	private MoveScript moveScript; // Used to move the character
	
	// Get player object to determine player position
	GameObject player;

	// Used to calculate player vs. enemy positions
	private Vector2 aiPosition;
	private Vector2 playerPosition;
	private float playerDistance;

	// Figure out which direction the enemy is facing
	public Vector2 facing = new Vector2(-1, 0);

	// Animations
	private bool isMoving = false;
	private bool isAttacking = false;



	public Vector2 randomXY;
	public Vector2 direction;


	public float moveCooldown;

	void Awake(){
		animator = GetComponent<Animator>();
		moveCooldown = 0f;

		player = GameObject.Find ("Player");
		aiPosition = new Vector2(transform.position.x , transform.position.y);
		playerPosition = new Vector2 (player.transform.position.x, player.transform.position.y);

	}
	
	void Update() {
		playerDistance = Vector3.Distance (player.transform.position, transform.position);
		if (playerDistance <= 1f) {
						isAttacking = true;
				}
		if (playerDistance > 1f)
						isAttacking = false;
		if(moveCooldown > 0) {
			moveCooldown -= Time.deltaTime;
		}

		if (moveCooldown <= 0) {
		isMoving = true;
		float x = Random.Range (-50, 50);
		float y = Random.Range (-50, 50);
		randomXY = new Vector2 (x, y);
		
			if (rigidbody2D.velocity.normalized.x != 0 || rigidbody2D.velocity.normalized.y != 0) {
			// Store the direction the player is facing in case they stop moving
			facing = rigidbody2D.velocity.normalized;
			} 

		direction = randomXY - aiPosition;
		direction = direction.normalized;

		

//		aiPosition = direction * speed;
		
		/* Play walking animation */
		animator.SetBool ("isMoving", isMoving);
	
		//Play idle animation 
		//	animator.SetFloat ("facing_x", facing.x);
		//	animator.SetFloat ("facing_y", facing.y);
				} else {
						isMoving = false;
						animator.SetBool ("isMoving", isMoving);		
		}
		animator.SetBool ("isAttacking", isAttacking);
}

	void FixedUpdate(){
			rigidbody2D.AddForce(aiPosition);
		}
}
