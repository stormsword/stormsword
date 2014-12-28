using UnityEngine;
using System.Collections;

/* ChargeCommand - Charges towards the target */
public class ChargeCommandScript : CommandScript {

	private Vector2 direction;

	private MoveScript moveScript;

	/* ChargeCommandScript - Constructor called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public ChargeCommandScript(GameObject _character) : base(_character) {
		moveScript = character.GetComponent<MoveScript>();
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			if(target != null) {
				direction = GetDirection(target);
				moveScript.Move(direction);
			}
		}
	}

	/* GetDirection - Determines the direction the character should move to reach the target character
	 * GameObject target - the character you're trying to charge wildly towards!
	 */
	private Vector2 GetDirection(GameObject target) {
		return(target.transform.position - character.transform.position);
	}
}
