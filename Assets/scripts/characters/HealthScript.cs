using UnityEngine;
using System.Collections;

/* HealthScript - Gives an object HP */

public enum dmgTypes { Damage, Heal };

public class HealthScript : MonoBehaviour {

	// Total Hitpoints
	public float hp = 1;

	// Total Armor
	public float armor = 1;

	// Damage after all modifiers are calculated
	public float totalShotDamage;

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
		WeaponScript shot = otherCollider.gameObject.GetComponent<WeaponScript>();
		if (shot != null) {
			// Ignore friendly fire
			//Debug.Log (shot);
			if(shot.ownerType != gameObject.tag) {
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
				/* Attack should knock player back on impact */
				Knockback (transform, otherCollider.transform, 2000);	//needs looking into
				shot.damage = shot.damage - armor;
				if(shot.damage <= 0)
					shot.damage = 1;
				Damage (shot.damage);
			}
		}
		else{
			//Debug.Log(shot);
		}
	}

	/* Damage - Inflicts damage and check if the object should be destroyed */
	public void Damage(float damageCount) {
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
