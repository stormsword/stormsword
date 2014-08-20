using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CommandStackScript - A Stack of commands that allow an enemy to figure out what he/she/it should be doing at any given time */
public class CommandStackScript {
	
	// The data structure of commands
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
		if(currentCommand != null) {
			currentCommand.Execute();
		}
	}

	public CommandScript currentCommand {
		get {
			if(commands.Count > 0) {
				return(commands.Peek());
			}
			else {
				// Stack is empty
				return(null);
			}
		}
	}
}