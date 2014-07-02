using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Components

	private MoveScript moveScript;

	// Speed of the player
	public Vector2 speed = new Vector2(50, 50);

	void Awake() {

		moveScript = GetComponent<MoveScript>();

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Retrieve axis information from controller
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Calculate movement per-direction
		moveScript.direction = new Vector2(inputX, inputY);

		// Shooting!
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");

		if(shoot) {
			// User clicked 'fire'
			WeaponScript weapon = GetComponent<WeaponScript>();

			// If player has weapon equipped
			if(weapon != null) {
				weapon.Attack (false);	// False because player is not an enemy
			}
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

	// Physics calcs
	void FixedUpdate() {
		// Move the game object
	}
}
