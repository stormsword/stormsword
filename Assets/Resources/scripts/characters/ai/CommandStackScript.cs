using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CommandStackScript - A Stack of commands that allow an enemy to figure out what he/she/it should be doing at any given time */
public class CommandStackScript {
	
	private Stack<CommandScript> commands;
	
	/* Constructor - Creates a new CommandStack */
	public CommandStackScript() {
		commands = new Stack<CommandScript>();
		
		MoveCommandScript command = new MoveCommandScript();
		command.direction = new Vector2(0, 1);
		commands.Push(command);
	}
	
	/* Execute - Run the top command on the stack */
	public void Execute () {
		// Run the given command this frame
		Debug.Log (commands.Peek().direction);
	}
}