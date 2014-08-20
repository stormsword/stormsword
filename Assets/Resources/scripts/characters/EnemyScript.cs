using UnityEngine;
using System.Collections;

/* EnemyScript - Controls enemy behavior */

public enum Archetypes {
	Stalker,
	Wanderer
}

[System.Serializable]
public class EnemyArchetype{
	public Archetypes movementType;
	internal Vector2 spawnPoint;
}

public class EnemyScript : MonoBehaviour {

	public EnemyArchetype enemyArchetype;

	// AI Scripts - Used to determine character behavior
	private CommandStackScript commands;

	private CharacterScript characterScript;



	// String to define AI movement

	void Start() {

		characterScript = GetComponent<CharacterScript>();

		commands = new CommandStackScript();

		MoveCommandScript command = new MoveCommandScript(this.gameObject);	// Temporary command - move to a random spot
		command.target = GameObject.FindGameObjectWithTag("Player");		// Grab and target player
		commands.Add(command);
	}

	void Update() {

		/* Update the command stack */
		switch(enemyArchetype.movementType) {
			case Archetypes.Stalker:
				break;
			case Archetypes.Wanderer:
				break;
		}

		/* Process the latest command stack */
		// Continuously attack until dead
		characterScript.Attack();

		commands.Execute();	// Execute our currently active command
	}
}
