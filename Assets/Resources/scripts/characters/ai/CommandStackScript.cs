using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CommandStackScript - A Stack of commands that allow an enemy to figure out what he/she/it should be doing at any given time */
public class CommandStackScript : MonoBehaviour {
	
	private Stack<CommandScript> commands;
	
	// Use this for initialization
	void Start () {
		commands = new Stack<CommandScript>();
		
		CommandScript command = new CommandScript();
		
		commands.Push(command);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (commands.ToString());
	}
}