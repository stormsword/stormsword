using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

	// Components
	private MoveScript parentMoveScript;
	private MoveScript shotMoveScript;
	private ItemSlotScript mainHandSlot;

	// Projectile prefab for shooting
	public Transform shotPrefab;

	// Weapon stats
	public float shootingRate = 0.25f; // Cooldown between attacks
	public float damage = 1;	// Damage a weapon does per attack
	public float radius = 5; 	// Radius the weapon affects upon impact
	public string type = "Melee";	// Melee or Ranged
	
	// Use this for initialization
	void Start () {
		mainHandSlot = transform.GetComponent<ItemSlotScript>();	// Grab the parent mainhand to get any slot-related info

	}

	void Update () {
	}

	/* Attack - Shot triggered by another script */
	public void Attack() {
		if(CanAttack) {
			// Character attacked, trigger cooldown
			mainHandSlot.Cooldown(shootingRate);

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Figure out what direction character is facing
			parentMoveScript = transform.parent.GetComponent<MoveScript>();

			Vector3 facing = new Vector3(parentMoveScript.facing.x, parentMoveScript.facing.y, 0);

			// Grab the position of the parent object (transform)
			shotTransform.position = transform.position + facing;

			// Get the shot's move script to adjust its direction
			shotMoveScript = shotTransform.GetComponent<MoveScript>();

			if(shotMoveScript) {
				shotMoveScript.direction = facing;
			}

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
			return(mainHandSlot.attackCooldown <= 0f);
		}
	}
}
