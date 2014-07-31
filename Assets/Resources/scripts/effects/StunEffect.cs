using UnityEngine;
using System.Collections;

/* Stun Effect - Knocks back a character away from the current character */

public class StunEffect : Effect {
	public float amount;	// Amount (#) multiplier on the movement vector

	private MoveScript targetMoveScript;	// The target's movement script
		
	protected override void ApplyEffect() {
	}
}