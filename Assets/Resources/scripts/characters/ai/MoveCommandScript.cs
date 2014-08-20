using UnityEngine;
using System.Collections;

/* MoveCommand - Moves a character towards a Vector2 */
public class MoveCommandScript : CommandScript {

	// Update is called once per frame
	protected override void Execute () {
		Debug.Log("Executing!");
		Debug.Log (direction);
	}
}
