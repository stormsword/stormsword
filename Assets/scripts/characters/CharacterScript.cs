using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	public string characterType;	// Enemy or Player

	private WeaponScript[] weapons;

	private ItemSlotScript items;
	
	void Awake() {
		// Grab the weapon once when the enemy spawns
		items = GetComponentInChildren<ItemSlotScript> ();

		weapons = items.GetComponentsInChildren<WeaponScript>();

	}
	
	void Update() {

	}

	public void Attack() {
		// Fire all equipped weapons
		foreach(WeaponScript weapon in weapons) {
			// Auto-fire
			if(weapon != null && weapon.CanAttack) {
				weapon.Attack();
			}
		}
	}
}
