using UnityEngine;
using System.Collections;

/* HealthScript - Gives an object HP */

public enum dmgTypes { Damage, Heal };

public class HealthScript : MonoBehaviour {

	// Total Hitpoints
	public int hp = 1;

	// Total Armor
	public int armor = 1;

	// Damage after all modifiers are calculated
	public int totalShotDamage;

	// Enemy or Player?
	public bool isEnemy = true;

	// HP Bubble displayed when a character takes dmg
	public Transform HPBubble;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D otherCollider) {
		// Is this a shot?
		ProjectileScript shot = otherCollider.gameObject.GetComponent<ProjectileScript>();
		
		if (shot != null) {
			// Ignore friendly fire
			if(shot.ownerType == "Player" && gameObject.tag == "Enemy") {
				// Player is attacking an enemy
				
				/* Attack should knock character back on impact */
				Knockback (transform, otherCollider.transform, 500);
				totalShotDamage = shot.damage - armor;
				if(totalShotDamage <= 0)
					// Damage is negative or zero
					totalShotDamage = 1;
				Damage (totalShotDamage);		// Target takes dmg
				totalShotDamage = shot.damage;	// Reset for shooting multiple enemies
					
			}

			if(shot.ownerType == "Enemy" && gameObject.tag == "Player"){
				// Enemy is attacking the Player
				shot.damage = shot.damage - armor;
				if(shot.damage <= 0)
					shot.damage = 1;
				Damage (shot.damage);
			}
		}
		else{
		
		}
	}

	/* Damage - Inflicts damage and check if the object should be destroyed */
	public void Damage(int damageCount) {
		hp -= damageCount;

		// Spawn -hp bubble
		var hpBubble = Instantiate(HPBubble) as Transform;
		hpBubble.transform.parent = transform;	// Make the current character its' parent

		// Set type of attack
		var hpBubbleScript = hpBubble.GetComponent<hpBubbleScript>();
		hpBubbleScript.dmgType = dmgTypes.Damage;
		hpBubbleScript.amount = damageCount;

		// Handle death

		if (hp <= 0) {
			// Object is dead
			Destroy(gameObject);
		}
	}

	/* Knockback - Knocks a unit (defender) away from a shot (shotTransform) by amount */
	public void Knockback(Transform defenderTransform, Transform shotTransform, float amount) {
		Vector2 direction = (defenderTransform.position - shotTransform.position).normalized;
		rigidbody2D.AddForce(direction * amount);
	}

}
