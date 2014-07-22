using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Components
	private CharacterScript characterScript;
	private MoveScript moveScript;
	private Animator animator;

	// Animations
	private bool playerAttack = false;


	// Speed of the player
	public Vector2 speed = new Vector2(50, 50);

	void Awake() {
		moveScript = GetComponent<MoveScript>();
		characterScript = GetComponent<CharacterScript>();
		animator = GetComponent<Animator> ();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		/* Player Input */

		// Retrieve axis information from keyboard
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Calculate movement per-direction and move the player when a key is pressed
		moveScript.Move(inputX, inputY);

		// Retrieve button information from mouse
		bool attack = Input.GetButtonDown("Fire1");
		attack |= Input.GetButtonDown("Fire2");

		if(attack) {
			// Tell the character to attack

			/*Play Attack animation */
			playerAttack = true;
			animator.SetBool("playerAttack", playerAttack);
			characterScript.Attack();

		}
		else{

			playerAttack = false;
			animator.SetBool("playerAttack", playerAttack);
		}

		// Make sure player cannot leave the camera view
		var dist = (transform.position - Camera.main.transform.position).z;

		var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
		var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

		var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
		var bottomBorder = Camera.main.ViewportToWorldPoint (new Vector3(0, 1, dist)).y;

		transform.position = new Vector3(
			Mathf.Clamp (transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp (transform.position.y, topBorder, bottomBorder),
			transform.position.z);

	}
}
