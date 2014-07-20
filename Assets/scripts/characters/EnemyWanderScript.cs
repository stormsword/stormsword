using UnityEngine;
using System.Collections;

/* EnemyWanderScript moves the enemy at random */

public class EnemyWanderScript : MonoBehaviour {


	private Animator animator;

	public float speed = 20;

	GameObject enemy;

	private Vector2 aiPosition;

	public Vector2 facing = new Vector2(-1, 0);

	private bool isMoving = false;



	public Vector2 randomXY;
	public Vector2 direction;

	public float moveCooldown;

	void Awake(){
		animator = GetComponent<Animator>();
		moveCooldown = 0f;

		enemy = GameObject.Find("Brute");
		aiPosition = new Vector2(enemy.transform.position.x , enemy.transform.position.y);

	}
	
	void Update() {

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

						aiPosition = direction * speed;
						/* Play walking animation */
	
	
						animator.SetBool ("isMoving", isMoving);

						animator.SetFloat ("movement_x", randomXY.x);
						animator.SetFloat ("movement_y", randomXY.y);
	
						//Play idle animation 
						animator.SetFloat ("facing_x", facing.x);
						animator.SetFloat ("facing_y", facing.y);
				} else {
						isMoving = false;
						animator.SetBool ("isMoving", isMoving);		
		}
}

	void FixedUpdate(){
			rigidbody2D.AddForce(aiPosition);
		}
}
