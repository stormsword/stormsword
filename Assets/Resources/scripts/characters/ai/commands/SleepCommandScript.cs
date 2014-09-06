using UnityEngine;
using System.Collections;

/* SleepCommand - Sleeps - no movement or action is taken */
public class SleepCommandScript : CommandScript {

	MoveScript moveScript;

	/* SleepCommandScript - Constructor called when Command is first created 
	 _character - the Character that the Command Stack is attached to */
	public SleepCommandScript(GameObject _character) : base(_character) {
	}

	/* Execute is usually called once per frame */
	public override void Execute () {
		// do nothing! :)
	}
}
