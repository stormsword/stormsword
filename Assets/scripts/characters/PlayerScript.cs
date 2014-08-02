using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Components
	private CharacterScript characterScript;
	private MoveScript moveScript;
	private Animator animator;

	// Animations
	private bool playerAttack = false;

	// Hotkeys
	public char abilityKey = 'Q';
	
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

		// Watch for ability input
		bool ability = Input.GetKeyDown(KeyCode.Q);

		if(attack) {
			playerAttack = true;	// Used for attack animation
			characterScript.Attack();
		}
		else{
			// Character isn't attacking
			playerAttack = false;
		}

		if(ability) {
			// Player is executing ability
			characterScript.Ability();
		}

		animator.SetBool("playerAttack", playerAttack);

		// Make sure player cannot leave the camera view
//		var dist = (transform.position - Camera.main.transform.position).z;
//
//		var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
//		var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
//
//		var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
//		var bottomBorder = Camera.main.ViewportToWorldPoint (new Vector3(0, 1, dist)).y;
//
//		transform.position = new Vector3(
//			Mathf.Clamp (transform.position.x, leftBorder, rightBorder),
//			Mathf.Clamp (transform.position.y, topBorder, bottomBorder),
//			transform.position.z);

	}
}
