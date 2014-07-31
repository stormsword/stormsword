using UnityEngine;
using System.Collections;

/* DpT Effect - Apply Damage to a character over time */

public class HasteEffect : Effect {
	public float amount;	// Amount (%)

	private MoveScript targetMoveScript;	// The target's health script
		
	protected override void ApplyEffect() {
		if(targetMoveScript == null) {
			// Only get target movescript once
			targetMoveScript = target.GetComponent<MoveScript>();	// Damage() is part of HealthScript
		}

		// Apply damage to the target (will happen once per tick)
		targetMoveScript.speed *= 3;	// Apply damage to the target (mitigation is handled by HealthScript)
	}

	protected override void EndEffect() {
		targetMoveScript.speed = targetMoveScript.speed / 3;
	}
}