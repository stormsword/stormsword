using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {

	private WeaponScript[] weapons;
	
	void Awake() {
		// Grab the weapon once when the enemy spawns
		weapons = GetComponentsInChildren<WeaponScript>();
	}
	
	void Update() {

	}

	public void Shoot() {
		// Fire all equipped weapons
		foreach(WeaponScript weapon in weapons) {
			// Auto-fire
			if(weapon != null && weapon.CanAttack) {
				weapon.Attack(true);
			}
		}
	}
}
