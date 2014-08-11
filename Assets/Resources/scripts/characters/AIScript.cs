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


	/* Class movement definition
	 * Controls which AI script will be used
	 * Sets an anchor to return the enemy to if needed
	 */

	public enum Archetypes {
		Stalker,
		Wanderer
	}

	[System.Serializable]
	public class enemyArchetype{
		public Archetypes movementType;
		internal Vector2 spawnPoint;
	}

	// Archetype determines how the AI behaves
	public enemyArchetype Archetype;

	// Used to calculate player vs. enemy positions
	private Vector2 aiPosition;
	private float playerDistance;
	private float stalkerDistanceFromStart;
	private Vector2 playerPosition;

	// Figure out which direction the enemy is facing
	internal Vector2 facing;

	// Animation Triggers
	private bool isMoving = false;
	private bool isAttacking = false;


	// Hold two random numbers
	private Vector2 randomXY;

	// Move the monster
	internal Vector2 direction;
	private Vector2 stalkerDirection;
	private Vector2 ranDirection;

	// Stop moving
	private Vector2 stall;
	
	internal float moveCooldown;

	void Start(){
		animator = GetComponent<Animator>();

		moveScript = GetComponent<MoveScript>();

		characterScript = GetComponent<CharacterScript>();

		moveCooldown = 0f;

		//Class variable initialization 
		Archetype.spawnPoint = new Vector2 (transform.position.x, transform.position.y);

		direction = new Vector2(0,0);
		stall = new Vector2 (0, 0);

		player = GameObject.FindWithTag("Player");	// Check for player once, not every frame
	}

	void Update() {
		if(player != null){
			// Is the player still alive?

			aiPosition = new Vector2(transform.position.x , transform.position.y);
				// Get the exact point where the monster is located
			playerDistance = Vector3.Distance(player.transform.position, aiPosition);
				// Take the difference of the player location and the monster location

			if(playerDistance <= 1.5f){
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

			if (moveCooldown <= 0) {
				// Monster ready to move

				if(Archetype.movementType == Archetypes.Stalker){
					// Enable stalker movement
					if(playerDistance >= 3f){
						// Player too far, leashing monster
						isMoving = true;
						animator.SetBool ( "isMoving", isMoving);
						stalkerDistanceFromStart = Vector3.Distance(Archetype.spawnPoint, aiPosition);
	
							if(stalkerDistanceFromStart >= .2f){
							// Find a suitable stopping distance to keep animation fluid
							ranDirection = Archetype.spawnPoint - aiPosition;
							ranDirection = ranDirection.normalized;
							moveScript.Move (ranDirection.x, ranDirection.y);
							}
							
							else{
							isMoving = false;
							animator.SetBool ("isMoving", isMoving);
							moveScript.Move (stall.x, stall.y);
							}
					}

					else{
						isMoving = true;
						// Modify boolean for animation trigger
						playerPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
						stalkerDirection = playerPosition - aiPosition;
						// Distance to move is the players position as a vector - where the AI is right now
						stalkerDirection = stalkerDirection.normalized;
						// Normalize it
						moveScript.Move (stalkerDirection.x, stalkerDirection.y);	
						// Call moveScript move function
					}
				}

				else {
					//Wanderer Script

					if(playerDistance >= 4f){
					// Player out of range, stop moving
					isMoving = false;
					animator.SetBool ( "isMoving", isMoving);
					moveScript.Move (stall.x, stall.y);
					}

					else{
					// Wanderer Movement
						isMoving = true;
						// Modify boolean for animation trigger
						float x = Random.Range (-50, 50);
						float y = Random.Range (-50, 50);
						// Create two random numbers;
						randomXY = new Vector2 (x, y);
							
						direction = randomXY - aiPosition;
						// Distance to move is random vector - where the AI is right now
						direction = direction.normalized;
						moveScript.Move (direction.x, direction.y);	
						// Call moveScript move function
					}
				}
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
