using UnityEngine;
using System.Collections;

/* PlayerScript - Handle Input from a player */
public class PlayerScript : MonoBehaviour {

	// Hotkeys
	public KeyCode[] abilityKeys = new KeyCode[2];
	public KeyCode pauseKey;

	// Components
	private CharacterScript characterScript;
	private MoveScript moveScript;
	private Animator animator;

	// Animations
	private bool playerAttack = false;

	// Movement / Attack Code
	private bool inputEnabled = true;	// Should we allow the player to Move/etc
	
	void Awake() {
		moveScript = GetComponent<MoveScript>();
		characterScript = GetComponent<CharacterScript>();
		animator = GetComponent<Animator>();
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		/* Player Input */
		// Retrieve axis information from keyboard
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Retrieve button information from mouse
		bool attack = Input.GetButtonDown("Fire1");
		attack |= Input.GetButtonDown("Fire2");

		// Watch for ability input
		bool ability1 = Input.GetKeyDown(abilityKeys[0]);
		bool ability2 = Input.GetKeyDown(abilityKeys[1]);	// Todo - figure out how to assign this dynamically

		bool pause = Input.GetKeyDown(pauseKey);

		if(inputEnabled) {
			// Calculate movement per-direction and move the player when a key is pressed
			moveScript.Move(inputX, inputY);

			if(attack) {
				playerAttack = true;	// Used for attack animation
				characterScript.Attack();
			}
			else{
				// Character isn't attacking
				playerAttack = false;
			}

			if(ability1) {
				// Player is executing ability #0
				characterScript.Ability(0);
			}

			if(ability2) {
				// Player is executing ability #1
				characterScript.Ability(1);
			}

			if(pause) {
				// Player is trying to pause the game
				characterScript.Pause();
			}

			animator.SetBool("playerAttack", playerAttack);
		}
		else {
			// Player should NOT move when paused
			moveScript.Move (0, 0);
		}
	}

	/* ToggleInput - Enables or Disables player movement/attack input while retaining ability to open the menu */
	public void ToggleInput() {
		if(inputEnabled) {
			inputEnabled = false;
			Debug.Log ("Disabled Input");
		}
		else {
			inputEnabled = true;
			Debug.Log ("Enabled Input");
		}
	}
}
