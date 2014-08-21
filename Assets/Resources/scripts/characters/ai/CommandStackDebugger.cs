using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/* CommandStackDebugger - Visualize the Command Stack in-game for each enemy */
public class CommandStackDebugger : MonoBehaviour {

	[Tooltip("Size of the stack graphic")]
	public Vector3 size = new Vector3(1, 1, 1);

	private Stack<CommandScript> stackCopy;
	private EnemyScript enemyScript;

	private void Start() {
		enemyScript = GetComponent<EnemyScript>();
		if(enemyScript != null) {
			stackCopy = this.enemyScript.commands.commands;
		}
	}

	/* OnDrawGizmos - Used to draw debugging info on the scene */
	public void OnDrawGizmos() {
		Gizmos.color = Color.grey;

		Handles.Label (transform.position, stackCopy.Peek().ToString());
	}
}
