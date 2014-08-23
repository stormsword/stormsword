using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CommandStackScript - A Stack of commands that allow an enemy to figure out what he/she/it should be doing at any given time 
	* Commands are added to the stack if a given condition is met (i.e. Player enters aggro radius)
	* Commands are removed from the stack if they meet their goal (i.e. 'walk to this point', or 'open a door')
 */
public class CommandStackScript {
	
	// The data structure to hold our commands
	internal Stack<CommandScript> commands;

	/* Constructor - Creates a new CommandStack */
	public CommandStackScript() {
		commands = new Stack<CommandScript>();
	}
	
	/* currentcommand - Stores the command currently at the top of the stack. 
	 * Returns null if stack is empty */
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
	
	/* Execute - Run the top command on the stack */
	public void Execute () {
		// Run the given command this frame
		if(currentCommand != null) {
			if(currentCommand.isComplete) {
				// Current task is finished, pop it off!
				Remove();
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
				commands.Push(command);
			}
		}
		else {
			// The stack is empty so push it!
			commands.Push(command);
		}
	}

	/* Remove - Pops the top command off the stack */
	public void Remove() {
		if(currentCommand != null) {
			commands.Pop ();
		}
	}

	/* ToString - Exports a string representing the current stack */
	public override string ToString() {
		Stack<CommandScript> tmpStack = new Stack<CommandScript>(commands);	// Create a temporary copy of the command stack

		string debugger = "Stack Contents: [" + tmpStack.Count + "]\n";
		while(tmpStack.Count > 0) {
			CommandScript popped = tmpStack.Pop();
			debugger += popped.ToString();
			debugger += "\n";
		}
		return(debugger);
	}

}