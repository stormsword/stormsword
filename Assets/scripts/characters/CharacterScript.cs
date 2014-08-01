using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	public string characterType;	// Enemy or Player
	
	private WeaponScript[] weapons;
	private AbilityScript[] abilities;

	private ItemSlotScript mainhand;
	private AbilitySlotScript abilitySlot;

	void Awake() {
		// Grab the weapon once when the enemy spawns
		mainhand = GetComponentInChildren<ItemSlotScript>();
		abilitySlot = GetComponentInChildren<AbilitySlotScript>();

		if(mainhand != null) {
			// If character has a slot for weapons)
			weapons = mainhand.GetComponentsInChildren<WeaponScript>();
		}

		if(abilitySlot != null) {
			// Get equipped ability
			abilities = abilitySlot.GetComponentsInChildren<AbilityScript>();

//			stomp = abilitySlot.GetComponentInChildren<StompScript>();
		}

	}
	
	void Update() {
	}

	public void Attack() {
		if(weapons != null) {
			// Fire all equipped weapons
			foreach(WeaponScript weapon in weapons) {
				// Auto-fire
				if(weapon != null && weapon.CanAttack) {
					weapon.Attack();
				}
			}
		}
	}

	/* Ability - Uses ability in # slot
	 * int slot - the slot # of the current ability
	*/
	internal void Ability(int slot) {
		if(abilities[slot] != null) {
			if(abilitySlot.CanCast) {
				abilities[slot].Cast();
			}
		}
	}
}
