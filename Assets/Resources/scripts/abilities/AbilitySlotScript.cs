using UnityEngine;
using System.Collections;

/* AbilitySlotScript - Allows a character to equip an ability */
public class AbilitySlotScript : MonoBehaviour {

	// Remaining cooldown for using ability in slot 1
	public float abilityCooldown;
	
	// Use this for initialization
	void Start () {
		abilityCooldown = 0f;	// No ability has been used yet so default to zero
	}
	
	// Update is called once per frame
	void Update () {
		// If slot has a cooldown, slowly decrement the cooldown until it reaches zero
		if(abilityCooldown > 0) {
			abilityCooldown -= Time.deltaTime;
		}
		if (abilityCooldown < 0)
			abilityCooldown = 0;
	}

	// An item in this slot triggered the slot's cooldown
	public void Cooldown(float amount) {
		abilityCooldown = amount;
	}
}
