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

	// HP Bubble displayed when a character takes dmg
	[Tooltip("Drag a prefab here for the health bubble UI to display for this player")]
	public Transform HPBubble;

	// Deathscript allows characters to have custom death sequences
	private DeathScript deathScript;

	/* Damage - Inflicts damage and check if the object should be destroyed */
	public void Damage(float damageCount) {
		if(hp > 0) {
			// Object is alive
			hp -= damageCount;

			// Spawn -hp bubble
			var hpBubble = Instantiate(HPBubble) as Transform;
			hpBubble.transform.parent = transform;	// Make the current character its' parent

			// Set type of attack
			var hpBubbleScript = hpBubble.GetComponent<hpBubbleScript>();
			hpBubbleScript.dmgType = dmgTypes.Damage;
			hpBubbleScript.amount = damageCount;
		}
		else {
			// Object is dead
			deathScript = GetComponent<DeathScript>();
			if(deathScript) {
				deathScript.enabled = true;	// Deathscript is disabled by default, enabling it triggers deathstuff
				this.enabled = false;
			} else {
				// Object doesn't have a deathScript so just destroy it
				GameObject.Destroy(gameObject);
			}
			
		}
	}
}
