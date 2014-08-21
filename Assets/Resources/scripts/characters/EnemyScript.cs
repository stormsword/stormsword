using UnityEngine;
using UnityEditor;
using System.Collections;

/* EnemyScript - Controls enemy behavior */
public enum Archetypes {
	Stalker,
	Wanderer
}

[System.Serializable]
public class EnemyArchetype{
	public Archetypes movementType;
}

public class EnemyScript : MonoBehaviour {

	[Tooltip("How does this enemy behave towards players?")]
	public EnemyArchetype enemyArchetype;

	[Tooltip("How close does a player have to be to aggro this enemy")]
	public float aggroRadius = 1.5f;

	[Tooltip("How close does a player have to be to enter this enemy's Line of Sight")]
	public float sightRadius = 3.0f;

	[Tooltip("Turn on debug mode to see a visual stack for each enemy. Must restart game to enable/disable!")]
	public bool debugMode = false;	// Debugging the Command Stack

	// Character's spawn point
	internal Vector2 spawnPoint;

	// AI Scripts - Used to execute character behavior
	internal CommandStackScript commands;
	private CharacterScript characterScript;

	// Player data - Fed to AI script to determine character behavior
	private GameObject player;
	private float playerDistance;


	void Start() {
		characterScript = GetComponent<CharacterScript>();

		// Grab player info
		player = GameObject.FindGameObjectWithTag("Player");

		// Save spawn point in case we need to leash later
		spawnPoint = transform.position;

		// Create stack of commands for AI behavior
		commands = new CommandStackScript();
	}

	void Update() {

		/* Update the command stack */
		if(player != null) {
			// Make sure player is still alive
			playerDistance = GetDistance (player);

			if(playerDistance <= aggroRadius) {
				// If character is within aggro radius, push 'charge' onto stack, regardless of archetype
				Charge();
			}
			else {
				// Otherwise revert to default behavior
				switch(enemyArchetype.movementType) {
					case Archetypes.Stalker:
						if(isVisible(player)) {
							// Stalker charges by default as long as player is on screen
							Charge ();
						}
						else {
							Goto(spawnPoint);
						}
						break;
					case Archetypes.Wanderer:
						// Wanderer just wanders by default
						Wander ();
						break;
				}
			}

			/* Process the latest command stack */
			commands.Execute();	// Execute our currently active command

			// Continuously attack until dead
			characterScript.Attack();
		}

	}

	/* Charge - Pushes a 'Charge' command onto the stack */
	private void Charge() {
		ChargeCommandScript command = new ChargeCommandScript(this.gameObject);	// Temporary command - move to a random spot
		command.target = player;		// Grab and target player
		commands.Add(command);			// Command will be executed next frame
	}

	/* Wander - Pushes a 'Wander' command onto the stack */
	private void Wander() {
		WanderCommandScript command = new WanderCommandScript(this.gameObject);
		commands.Add(command);
	}

	/* MoveTo - Travel to a location */
	private void Goto(Vector2 location) {
		GotoCommandScript command = new GotoCommandScript(this.gameObject);
		command.destination = location;
		commands.Add (command);
	}

	private float GetDistance(GameObject target) {
		return(Vector2.Distance(this.gameObject.transform.position, target.transform.position));
	}

	private bool isVisible(GameObject target) {
		if(GetDistance(target) <= sightRadius) {
			// Player is inside the target's sight radius, he can see you!
			return(true);
		}
		return(false);
	}

	/* OnDrawGizmos - Used to draw debugging info on the scene */
	public void OnDrawGizmos() {
		if(debugMode) {
			Gizmos.color = Color.grey;	
			Handles.Label (transform.position, commands.currentCommand.ToString());
		}
	}
}
