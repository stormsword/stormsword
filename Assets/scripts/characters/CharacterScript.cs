using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	public string characterType;	// Enemy or Player

	private WeaponScript[] weapons;

	private ItemSlotScript mainhand;
	
	void Awake() {
		// Grab the weapon once when the enemy spawns
		mainhand = GetComponentInChildren<ItemSlotScript>();

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
}
