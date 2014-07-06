using UnityEngine;
using System.Collections;

/* HealthScript - Gives an object HP */

public enum dmgTypes { Damage, Heal };

public class HealthScript : MonoBehaviour {

	// Total Hitpoints
	public int hp = 1;

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

	void OnTriggerEnter2D(Collider2D otherCollider) {
		// Is this a shot?
		ProjectileScript shot = otherCollider.gameObject.GetComponent<ProjectileScript>();

		if (shot != null) {
			// Ignore friendly fire
			if(shot.ownerType == "Player" && gameObject.tag == "Enemy") {
				// Player is attacking an enemy
				Damage (shot.damage);		// Target takes dmg
			}
		}
	}
}
