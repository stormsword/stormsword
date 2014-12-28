using UnityEngine;
using System.Collections;

/* WanderCommand - Wanders randomly around the current position */
public class WanderCommandScript : CommandScript {

	MoveScript moveScript;

	/* MoveCommandScript - Constructur called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public WanderCommandScript(GameObject _character) : base(_character) {
		moveScript = character.GetComponent<MoveScript>();
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			destination = GetRandomDestination();
			moveScript.Move(destination);
		}
	}

	private Vector2 GetRandomDestination() {
		return(Random.insideUnitCircle*3);
	}	
}
