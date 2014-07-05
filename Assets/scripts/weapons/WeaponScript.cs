using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

	// Components
	private MoveScript parentMoveScript;
	private MoveScript shotMoveScript;

	// Projectile prefab for shooting
	public Transform shotPrefab;

	// Weapon stats
	public float shootingRate = 0.25f; // Cooldown between attacks
	public float damage = 1;	// Damage a weapon does per attack
	public float radius = 5; 	// Radius the weapon affects upon impact
	public string type = "Melee";	// Melee or Ranged

	// Remaining cooldown for shot
	private float attackCooldown;

	// Use this for initialization
	void Start () {
		attackCooldown = 0f;	// Object has not yet shot
	}

	void Update () {
		if(attackCooldown > 0) {
			attackCooldown -= Time.deltaTime;
		}
	}

	/* Attack - Shot triggered by another script */
	public void Attack() {
		if(CanAttack) {
			// Character attacked, trigger cooldown
			attackCooldown = shootingRate;

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Grab the position of the parent object (transform)
			shotTransform.position = transform.position;

			// Figure out what direction character is facing

//			if(parentMoveScript) {
//				// Set the shot to face the same direction as the player
//				shotMoveScript = shotTransform.GetComponent<MoveScript>();
//			}


 			switch(type) {
			case "Melee": 
				// Handle melee weapon code here

				// Melee attack is attached to parent (character)
				shotTransform.parent = transform;

				break;

			case "Ranged": 
				// Handle ranged weapon code here
					break;	

			}

			// Fire the actual projectile
			ProjectileScript projectile = shotTransform.gameObject.GetComponent<ProjectileScript>();
	
			if(projectile != null) {
				// Determine what type of player shot this
				projectile.ownerType = gameObject.transform.parent.gameObject.tag;
			}
		}
	}

	// Is the weapon ready to create a new projectile?
	public bool CanAttack {
		get {
			return(attackCooldown <= 0f);
		}
	}
}
