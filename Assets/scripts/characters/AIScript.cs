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

		moveScript = GetComponent<MoveScript>();

		characterScript = GetComponent<CharacterScript>();

		moveCooldown = 0f;

		player = GameObject.Find ("Player");
		aiPosition = new Vector2(transform.position.x , transform.position.y);
		playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);

	}
	
	void Update() {

		// Get distance from player
		playerDistance = Vector3.Distance(player.transform.position, transform.position);

		if (playerDistance <= 1f) {
			characterScript.Attack();
		}

		if (playerDistance > 1f) {
			isAttacking = false;
		}

		if(moveCooldown > 0) {
			moveCooldown -= Time.deltaTime;
		}

		if (moveCooldown <= 0) {
			isMoving = true;
			float x = Random.Range (-50, 50);
			float y = Random.Range (-50, 50);
			randomXY = new Vector2 (x, y);

			direction = randomXY - aiPosition;
			direction = direction.normalized;
			
			moveScript.Move(direction.x, direction.y);
		}
		
	}

}
