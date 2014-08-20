using UnityEngine;
using System.Collections;

/* MoveCommand - Moves a character towards a Vector2 */
public class MoveCommandScript : CommandScript {

	MoveScript moveScript;

	/* MoveCommandScript - Constructur called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public MoveCommandScript(GameObject _character) : base(_character) {

	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			Debug.Log("Moving to....");
			Debug.Log (direction);
		}
	}
}
