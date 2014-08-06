using UnityEngine;
using System.Collections;

/* DpT Effect - Apply Damage to a character over time */

public class DoTEffect : Effect {

	[Tooltip("Amount (HP) of damage per tick")]
	public float amount;

	private HealthScript targetHealthScript;	// The target's health script
	
	protected override void ApplyEffect() {
		// Apply damage to the target (will happen once per tick)

		if(target) {
			// Check if target is still alive
			targetHealthScript = target.GetComponent<HealthScript>();	// Damage() is part of HealthScript

			targetHealthScript.Damage(amount);	// Apply damage to the target (mitigation is handled by HealthScript)
		}
		else {
			// Target is dead, get rid of the DoT
			base.EndEffect();
		}
	}
}