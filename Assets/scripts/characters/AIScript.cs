using UnityEngine;
using System.Collections;

/* AIScript moves the enemy at random */

public class AIScript : MonoBehaviour {

	/* Components */
	private Animator animator;
	private MoveScript moveScript; // Used to move the character
	private CharacterScript characterScript;	// Used to attack
	
	// Get player object to determine player position
	GameObject player;

	// Used to calculate player vs. enemy positions
	private Vector2 aiPosition;
	private float playerDistance;

	// Figure out which direction the enemy is facing
	public Vector2 facing;

	// Animations
	private bool isMoving = false;
	private bool isAttacking = false;


	// Hold two random numbers
	public Vector2 randomXY;

	// Move the monster
	public Vector2 direction;

	// Stop moving
	public Vector2 stall = new Vector2(0,0);


	public float moveCooldown;

	void Awake(){
		animator = GetComponent<Animator>();

		moveScript = GetComponent<MoveScript>();

		characterScript = GetComponent<CharacterScript>();

		moveCooldown = 0f;

		direction = new Vector2(0,0);

	}

	void Update() {
		player = GameObject.Find ("Player");


		if(player != null)
			// Is the player still alive?
		{
			aiPosition = new Vector2(transform.position.x , transform.position.y);
				// Get the exact point where the monster is located
			playerDistance = Vector3.Distance(player.transform.position, aiPosition);
				// Take the difference of the player location and the monster location


			if (playerDistance <= 1.5f) {
				// Monster is close enough to Initiate Melee attack
				isAttacking = true;
				animator.SetBool ("isAttacking", isAttacking);
				characterScript.Attack();
			}

			if(playerDistance > 1.5f){
				isAttacking = false;
				animator.SetBool ("isAttacking", isAttacking);
			}

			if (moveCooldown > 0) {
				// Monster not ready to move, subtract frame rendering time
				moveCooldown -= Time.deltaTime;
			
			}

			if (moveCooldown < 0)
				// Probably not needed but good practice I think?
				moveCooldown = 0;
				
		 	if (moveCooldown == 0) {
			// Monster ready to move

				isMoving = true;
				// Modify boolean for animation trigger
				float x = Random.Range (-50, 50);
				float y = Random.Range (-50, 50);
				// Create two random numbers;
				randomXY = new Vector2 (x, y);
			
				direction = randomXY - aiPosition;
				// Distance to move is random vector - where the AI is right now
				direction = direction.normalized;
				// Normalize it
				moveScript.Move (direction.x, direction.y);	//does this break out of update?
				// Call moveScript move function
				} 
				else {
				// Monster not ready to move
					isMoving = false;
					// Modify boolean for animation trigger
					animator.SetBool ("isMoving", isMoving);
					// Tell animator to stop movement
				}
			}

		else{
		// Player is dead :(
			isAttacking = false;
			isMoving = false;
			animator.SetBool ("isAttacking", isAttacking);
			animator.SetBool ("isMoving", isMoving);
			moveScript.Move (stall.x, stall.y);
			// Stop monster and animations, he won he deserves a break

		}
	}


}
