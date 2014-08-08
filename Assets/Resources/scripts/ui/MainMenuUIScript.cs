using UnityEngine;
using System.Collections;

/* MainMenuScript - Display UI for main menu */
public class MainMenuUIScript : MonoBehaviour {

	internal static MainMenuUIScript instance;

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

	void OnGUI() {
		// Draw black background over scene
		GUI.backgroundColor = Color.black;
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

		// Draw title text/image
		GUI.Label(titlePosition, titleText);

		// Draw start button
		GUI.contentColor = Color.white;
		bool startButton = GUI.Button(startPosition, startText);

		// Draw continue button
		GUI.contentColor = Color.grey;
		GUI.Label(continuePosition, continueText);	// Currently continue does not work.

		if(startButton) {
			// Player pressed the start button
			MainMenuScript.instance.StartGame();
		}
	}
}
