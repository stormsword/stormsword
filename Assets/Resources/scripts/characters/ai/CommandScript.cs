using UnityEngine;
using System.Collections;

/* CommandScript - Base Class to represent commands in the game. Usually added to a CommandStack */
public class CommandScript {

	internal Vector2 direction;
	internal GameObject character;

	protected bool isActive = true;	// Script is active when instantiated by default

	public CommandScript(GameObject _character) {
		this.character = _character;
	}

	public virtual void Execute() {
	}

	/* Pause - Stop the current script from Executing */
	public virtual void Pause() {
		isActive = false;
	}

	/* Resume - Start the current script up again */
	public virtual void Resume() {
		isActive = true;
	}
}
