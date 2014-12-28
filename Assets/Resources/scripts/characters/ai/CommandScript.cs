using UnityEngine;
using System.Collections;

/* CommandScript - Base Class to represent commands in the game. Usually added to a CommandStack */
public class CommandScript {

	internal Vector2 destination;		// The destination the stack is supposed to move to
	internal GameObject character;		// The Character this stack is attached to (usually an enemy)
	internal GameObject target;			// The character targeted (usually a player)

	internal bool isActive = true;		// Script is active when instantiated by default
	internal bool isComplete = false;	// The action will be "Pop'd" when complete

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
	
	public virtual void Finish() {
		this.isComplete = true;
	}

	public virtual void debug() {
	}
}
