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
	}
	
	/* Execute - Run the top command on the stack */
	public void Execute () {
		// Run the given command this frame
		if(currentCommand != null) {
			if(currentCommand.isComplete) {
				// Current task is finished, pop it off!
				Debug.Log ("Current command is complete!");
				commands.Pop();
			}
			else {
				currentCommand.Execute();
			}
		}
	}


	/* Add - Adds a command to the top of the stack, pausing the current item and executing it next frame */
	public void Add(CommandScript command) {
		if(commands.Count > 0) {
			// Make sure we're not just receiving the same action again
			if(currentCommand.GetType() != command.GetType()) {
				// Pause current command
				currentCommand.Pause();
				commands.Push(command);
				Debug.Log (command.GetType());
			}
		}
		else {
			// The stack is empty so push it!
			commands.Push(command);
			Debug.Log (command.GetType());
		}
	}

	/* Remove - Pops the top command off the stack */
	public void Remove() {
		if(currentCommand != null) {
			commands.Pop ();

			// Check if there is a currentItem
			if(currentCommand != null) {
				currentCommand.Resume();
			}
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