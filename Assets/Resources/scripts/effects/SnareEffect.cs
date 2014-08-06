using UnityEngine;
using System.Collections;

/* Snare Effect - Decrease movement speed of character by x% for a period of time */

public class SnareEffect : Effect {

	[Tooltip("Amount (%) by which we reduce the target's movement speed")]
	public float amount;

	private MoveScript targetMoveScript;	// The target's movement script
	
	protected override void ApplyEffect() {
		if(target) {
			// Apply the snare to the target
			targetMoveScript = target.GetComponent<MoveScript>();	// Movespeed is set on MoveScript

			targetMoveScript.speed *= amount;	// Reduce the target's movement script by a percentage
		}
		else {
			// Target is dead, get rid of effect
			base.EndEffect();
		}
	}

	protected override void EndEffect() {
		// Remove the snare from the target

		targetMoveScript.speed /= amount;	// Remove target's movement speed to previous amount

		base.EndEffect();	// Run the base class' endeffect script to remove the effect
	}
}
