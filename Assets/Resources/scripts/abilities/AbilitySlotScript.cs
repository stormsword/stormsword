using UnityEngine;
using System.Collections;

/* AbilitySlotScript - Allows a character to equip an ability */
public class AbilitySlotScript : MonoBehaviour {

	// Remaining cooldown for using ability in slot
	public float abilityCooldown;

	public Transform abilityEquipped;

	private AbilityUIScript abilityUI;
	
	// Use this for initialization
	void Start () {
		abilityCooldown = 0f;	// No ability has been used yet so default to zero
		abilityUI = GetComponent<AbilityUIScript>();
	}
	
	// Update is called once per frame
	void Update () {
		// If slot has a cooldown, slowly decrement the cooldown until it reaches zero
		if(abilityCooldown > 0) {
			abilityCooldown -= Time.deltaTime;
		}
		if (abilityCooldown < 0) {
			abilityCooldown = 0;
			abilityUI.FadeIn();
		}
	}

	/* Cast - Creates and activates the equipped ability */
	public void Cast() {
		if(CanCast) {
			var ability = Instantiate(abilityEquipped, transform.position, transform.rotation) as Transform; // Ability should spawn under the player
			ability.transform.parent = transform;	// Set ability to be a child of the ability slot (and thus the player)

			var abilityScript = ability.gameObject.GetComponent<AbilityScript>();
			Cooldown(abilityScript.cooldown);	// Trigger cooldown on this slot
		}
	}

	// Is the slot off cooldown and ready to be used?
	public bool CanCast {
		get {
			return(abilityCooldown <= 0f);	
		}
	}

	// An item in this slot triggered the slot's cooldown
	public void Cooldown(float amount) {
		abilityCooldown = amount;
		abilityUI.FadeOut();
	}



}
