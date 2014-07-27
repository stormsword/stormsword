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
	private float totalDamage;

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
		EffectScript ability = otherCollider.gameObject.GetComponent<EffectScript>();
		if(ability != null) {
			Debug.Log ("Ability used!");
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
}
