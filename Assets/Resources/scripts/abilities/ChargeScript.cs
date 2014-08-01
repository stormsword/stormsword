using UnityEngine;
using System.Collections;

/* ChargeEffectScript - Character charges forward and knocks out all enemies in its path */
public class ChargeScript : AbilityScript {

  public float amount = 0f;

  private MoveScript characterMoveScript;	// Move script so we can push the character

	protected override void UseAbility(GameObject character) {
		if(characterMoveScript == null) {
			// Only get target movescript once
			characterMoveScript = character.GetComponent<MoveScript>();	// Push() is part of MoveScript
		}
			
		Vector2 direction = characterMoveScript.facing;
		
		characterMoveScript.Push(direction, amount);

	}


}
