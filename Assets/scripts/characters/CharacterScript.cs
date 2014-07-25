using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	public string characterType;	// Enemy or Player
	
	private WeaponScript[] weapons;

	private ItemSlotScript mainhand;
	private AbilitySlotScript ability;
	
	void Awake() {
		// Grab the weapon once when the enemy spawns
		mainhand = GetComponentInChildren<ItemSlotScript>();
		ability = GetComponentInChildren<AbilitySlotScript>();

		if(mainhand != null) {
			// If character has a slot for weapons)
			weapons = mainhand.GetComponentsInChildren<WeaponScript>();
		}

		if(ability != null) {
			Debug.Log ("Player has ability equipped");
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

	internal void Ability() {
		if(ability != null) {
			Debug.Log("Ability triggered!");
		}
	}
}
