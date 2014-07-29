using UnityEngine;
using System.Collections;

/* Snare Effect - Decrease movement speed of character by x% for a period of time */

public class DoTEffect : Effect {
	public float amount;	// Amount (%)

	private HealthScript targetHealthScript;	// The target's movement script
	
	protected override void ApplyEffect() {
		// Apply the snare to the target

		targetHealthScript = target.GetComponent<HealthScript>();	// Movespeed is set on MoveScript

		targetHealthScript.Damage(amount);	// Reduce the target's movement script by a percentage
	}
}