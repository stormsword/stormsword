using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

	// Components
	private MoveScript parentMoveScript;
	private MoveScript shotMoveScript;
	private ItemSlotScript mainHandSlot;
	

	private float playerDistance;

	// Projectile prefab for shooting
	public Transform shotPrefab;

	// Weapon stats
	public float shootingRate = 0.25f; 	// Cooldown between attacks
	public float damage = 1;			// Damage a weapon does per attack
	public float radius = 5; 			// Radius the weapon affects upon impact
	public float knockback = 500f;		// Amount to knock the character back upon impact
	public string type = "Melee";		// Melee or Ranged
	public string ownerType = "Player";	// Is a player or an enemy carrying the weapon?
	
	// Use this for initialization
	void Start () {
		mainHandSlot = transform.parent.GetComponent<ItemSlotScript>();	// Grab the parent mainhand to get any slot-related info
	}

	void Update () {
	}

	/* Attack - Shot triggered by another script */
	public void Attack() {
		if(CanAttack) {
			// Character attacked, trigger cooldown
			mainHandSlot.Cooldown(shootingRate);

 			switch(type) {
			case "Melee": 
				// Handle melee weapon code here

				// Melee attack is attached to parent (character)

				break;

			case "Ranged": 
				// Handle ranged weapon code here
				if(ownerType == "Enemy")
				{
				// Create a new shot
				var shotTransform = Instantiate(shotPrefab) as Transform;
				

				// Figure out what direction character is facing
				if(parentMoveScript)
				{
				parentMoveScript = transform.parent.GetComponent<MoveScript>();
				
				Vector3 facing = new Vector3(parentMoveScript.facing.x, parentMoveScript.facing.y, 0);
				
				// Grab the position of the parent object (transform)
				shotTransform.position = transform.position;
				
				// Get the shot's move script to adjust its direction
				shotMoveScript = shotTransform.parent.GetComponent<MoveScript>();
				
				if(shotMoveScript) {
					shotMoveScript.direction = facing;
				}
				
				// Fire the actual projectile
				ProjectileScript projectile = shotTransform.gameObject.GetComponent<ProjectileScript>();
				
				if(projectile) {
					// Determine what type of player shot this
					projectile.ownerType = gameObject.transform.parent.parent.gameObject.tag;
				}
				}
				}
					break;	

			}


		}
	}

	// Is the weapon ready to create a new projectile?
	public bool CanAttack {
		get {
			return(mainHandSlot.attackCooldown <= 0f);
		
		}
	}

	void OnTriggerEnter2D(Collider2D defenderCollider) {

		HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();

		// Check if the object I'm colliding with can be damaged
		if(defenderHealth != null) {

			GameObject defender = defenderHealth.gameObject;

			if((ownerType == "Player" && defender.tag == "Enemy")
			   ||
			   (ownerType == "Enemy" && defender.tag == "Player")){

				// Attacks should knock the defender back away from attacker
				Knockback (transform, defender, knockback);

				// Calculate armor reduction
				float totalDamage = damage - defenderHealth.armor;

				// attacks should always do 1 dmg, even if they are very weak
				if(totalDamage <= 0) {
					totalDamage = 1;
				}

				defenderHealth.Damage(totalDamage);		// Apply damage to the defender
			}
		}
	}

	/* Knockback - Knocks a unit (defender) away from an attacker's position (attackerTransform) by amount */
	public void Knockback(Transform attackerTransform, GameObject defender, float amount) {
		Vector2 direction = (defender.transform.position - attackerTransform.position).normalized;

		Rigidbody2D defenderPhysics = defender.GetComponent<Rigidbody2D>();
		defenderPhysics.AddForce(direction * amount);
	}
}
