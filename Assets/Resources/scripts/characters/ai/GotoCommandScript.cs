using UnityEngine;
using System.Collections;

/* GotoCommand - Moves towards a destination */
public class GotoCommandScript : CommandScript {

	MoveScript moveScript;

	/* GotoCommandScript - Constructur called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public GotoCommandScript(GameObject _character) : base(_character) {
		moveScript = character.GetComponent<MoveScript>();
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			moveScript.Move(destination);
		}
	}
}
