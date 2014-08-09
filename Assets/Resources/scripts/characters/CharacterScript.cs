using UnityEngine;
using System.Collections;

/* CharacterScript - contains shared behavior for Characters (Enemies, Players, NPC's, etc) */

public class CharacterScript : MonoBehaviour {
	
	private WeaponScript[] weapons;

	private ItemSlotScript mainhand;
	private AbilitySlotScript[] abilitySlots;	// Many abilities can be equipped, one per slot

	private bool paused;

	void Awake() {
		// Grab the weapon once when the enemy spawns
		mainhand = GetComponentInChildren<ItemSlotScript>();
		abilitySlots = GetComponentsInChildren<AbilitySlotScript>();

		// Tell each abilityslot which # it is for UI formatting
		for(int i = 0; i < abilitySlots.Length; i++) {
			AbilityUIScript uiScript = abilitySlots[i].GetComponent<AbilityUIScript>();
			uiScript.abilityIndex = i;
		}


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

	/* Pause - Pauses and Unpauses the game */
	internal void Pause() {
		if(!paused) {
			// Pause the game
			paused = true;

			// Stop time
			Time.timeScale = 0;

			// Tell UI to display 'Pause menu'
			PauseMenuScript.instance.Pause();
		}
		else {
			// Unpause the game
			paused = false;

			// Resume time
			Time.timeScale = 1;

			// Tell UI to stop displaying 'Pause menu'
			PauseMenuScript.instance.UnPause();
		}
	}

	/* Quit - Quit the current game */
	internal void Quit() {
//		Quit();	// For some reason Quit() crashes unity.
	}
}
