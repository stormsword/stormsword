using UnityEngine;
using System.Collections;

/* EnemyScript - Generic Enemy Behavior */

public class EnemyScript : MonoBehaviour {

	private WeaponScript[] weapons;

	void Awake() {
		// Grab the weapon once when the enemy spawns
		weapons = GetComponentsInChildren<WeaponScript>();
	}

	void Update() {
		foreach(WeaponScript weapon in weapons) {
			// Auto-fire
			if(weapon != null && weapon.CanAttack) {
				weapon.Attack(true);
			}
		}
	}
}
