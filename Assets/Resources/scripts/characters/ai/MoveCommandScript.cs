using UnityEngine;
using System.Collections;

/* MoveCommand - Moves a character towards a Vector2 */
public class MoveCommandScript : CommandScript {

	MoveScript moveScript;

	/* MoveCommandScript - Constructur called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public MoveCommandScript(GameObject _character) : base(_character) {
		moveScript = character.GetComponent<MoveScript>();
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		if(this.isActive) {
			if(target != null) {
				destination = GetDestination(target);
				moveScript.Move(destination);
			}
		}
	}

	private Vector2 GetDestination(GameObject target) {
		return(target.transform.position - character.transform.position);
	}

	private float GetDistance(GameObject target) {
		return(Vector2.Distance(character.transform.position, target.transform.position));
	}
}
