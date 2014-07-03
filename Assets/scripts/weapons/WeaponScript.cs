using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

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
	public void Attack(bool isEnemy) {
		if(CanAttack) {
			// Character attacked, trigger cooldown
			attackCooldown = shootingRate;

			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Grab the position of the parent object (transform)
			shotTransform.position = transform.position;

			switch(type) {
			case "Melee": 
				// Handle melee weapon code here
				Debug.Log("Melee");
				// Melee attack is attached to parent (character)
				shotTransform.transform.parent = transform;

				break;

			case "Ranged": 
				// Handle ranged weapon code here
				Debug.Log ("Ranged");
					break;	

			}

			ProjectileScript projectile = shotTransform.gameObject.GetComponent<ProjectileScript>();
			// Shot exists
			if(projectile != null) {
				projectile.isEnemyShot = isEnemy;
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
