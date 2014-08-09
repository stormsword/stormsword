using UnityEngine;
using System.Collections;

/* ProjectileScript - Used for shooting projectiles across the screen */

public class ProjectileScript : MonoBehaviour {

	// Damage the object deals
	[Tooltip("Damage dealt by the projectile")]
	public int damage = 1;

	[Tooltip("Duration the projectile should remain in the game world after being fired")]
	public float duration = 0.25f;

	[Tooltip("Drag a spell effect here if you want the projectile to apply it upon impact.")]
	public Transform spellEffect;	// Optional - Does the shot apply an effect?

	// Who shot this projectile?
	internal string ownerType;
	
	private BoxCollider2D projectileCollider;
	
	private CharacterScript character;
	
	void Start () {
		character = GetComponentInParent<CharacterScript>();
		ownerType = character.gameObject.tag;	// Get the character's tag so we can decide who the ability should damage

		//projectileCollider = GetComponent<BoxCollider2D>();

		Destroy(gameObject, duration);	// Only lives for 20 seconds to prevent leaks
	}




void OnTriggerEnter2D(Collider2D defenderCollider) {
	
	HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();
	
	// Check if the object I'm colliding with can be damaged
	if(defenderHealth != null) {
		
		GameObject defender = defenderHealth.gameObject;
		
		if((ownerType == "Player" && defender.tag == "Enemy")
		   ||
		   (ownerType == "Enemy" && defender.tag == "Player")) {

			if(spellEffect != null) {
				// Apply effect to enemy
				var effect = Instantiate(spellEffect) as Transform;

				var effectScript = effect.gameObject.GetComponent<Effect>();
				effectScript.target = defender;
			}
			
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
}