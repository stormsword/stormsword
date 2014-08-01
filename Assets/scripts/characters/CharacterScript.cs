using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	public string characterType;	// Enemy or Player
	
	private WeaponScript[] weapons;

	private ItemSlotScript mainhand;
	private AbilitySlotScript[] abilitySlots;	// Many abilities can be equipped, one per slot

	void Awake() {
		// Grab the weapon once when the enemy spawns
		mainhand = GetComponentInChildren<ItemSlotScript>();
		abilitySlots = GetComponentsInChildren<AbilitySlotScript>();

		if(mainhand != null) {
			// If character has a slot for weapons)
			weapons = mainhand.GetComponentsInChildren<WeaponScript>();
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
		if(abilitySlots[slot] != null) {
			abilitySlots[slot].Cast();
		}
	}
}
