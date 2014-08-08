using UnityEngine;
using System.Collections;

/* MainMenuScript - Display main menu and allow player to start game (or continue.... one day!) */
public class MainMenuScript : MonoBehaviour {

	internal static MainMenuScript instance;

	// Main menu title
	public Rect titlePosition;
	public string titleText;

	// Start Game Button
	public Rect startPosition;
	public string startText;

	// Continue Game Button
	public Rect continuePosition;
	public string continueText;

	void Start (){
		instance = this;
	}

	void OnGui() {
		// Draw black background over scene
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

		// Draw title text/image
		GUI.Label(titlePosition, titleText);

		// Draw start button
		GUI.Button(startPosition, startText);

		// Draw continue button
		GUI.Button(continuePosition, continueText);
	}
	
	void Update () {
	
	}
}
