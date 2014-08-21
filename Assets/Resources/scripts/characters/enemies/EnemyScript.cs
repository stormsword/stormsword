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

	[Tooltip("How does this enemy behave towards players?")]
	public EnemyArchetype enemyArchetype;

	[Tooltip("How close does a player have to be to aggro this enemy")]
	public float aggroRadius = 1.5f;

	// AI Scripts - Used to execute character behavior
	private CommandStackScript commands;
	private CharacterScript characterScript;

	// Player data - Fed to AI script to determine character behavior
	private GameObject player;
	private float playerDistance;

	void Start() {
		characterScript = GetComponent<CharacterScript>();

		// Grab player info
		player = GameObject.FindGameObjectWithTag("Player");

		// Create stack of commands for AI behavior
		commands = new CommandStackScript();

		// Add a default command to the stack
		ChargeCommandScript command = new ChargeCommandScript(this.gameObject);	// Temporary command - move to a random spot
		command.target = player;		// Grab and target player
		commands.Add(command);			// Command will be executed next frame
	}

	void Update() {

		/* Update the command stack */
		playerDistance = GetDistance (player);

		if(playerDistance >= aggroRadius) {
			// If character is within aggro radius, push 'charge' onto stack, regardless of archetype

		}
		else {
			// Otherwise revert to default behavior
			switch(enemyArchetype.movementType) {
				case Archetypes.Stalker:
					break;
				case Archetypes.Wanderer:
					break;
			}
		}

		/* Process the latest command stack */
		commands.Execute();	// Execute our currently active command

		// Continuously attack until dead
		characterScript.Attack();

	}

	private float GetDistance(GameObject target) {
		return(Vector2.Distance(this.gameObject.transform.position, target.transform.position));
	}
}
