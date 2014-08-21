using UnityEngine;
using System.Collections;

/* GotoCommand - Moves towards a destination point */
public class GotoCommandScript : CommandScript {

	MoveScript moveScript;

	Vector2 direction;

	/* GotoCommandScript - Constructur called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public GotoCommandScript(GameObject _character) : base(_character) {
		moveScript = character.GetComponent<MoveScript>();
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			direction = GetDirection(destination);
			moveScript.Move(direction);
		}
	}
	
	private Vector2 GetDirection(Vector2 destination) {
		Vector2 targetPosition = new Vector2(character.transform.position.x, character.transform.position.y);	// Workaround because transform.position is a vector3
		return(destination - targetPosition);
	}

	public override void debug() {
		Debug.Log ("Current Location: ");
		Debug.Log (character.transform.position);
		Debug.Log ("Destination: ");
		Debug.Log(destination);
	}
}
