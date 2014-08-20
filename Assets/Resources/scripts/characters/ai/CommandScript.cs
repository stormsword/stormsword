using UnityEngine;
using System.Collections;

/* CommandScript - Base Class to represent commands in the game. Usually added to a CommandStack */
public class CommandScript {

	internal Vector2 direction;

	public CommandScript() {
	}

	public virtual void Execute() {
	}
}
