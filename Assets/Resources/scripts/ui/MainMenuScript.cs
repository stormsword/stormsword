using UnityEngine;
using System.Collections;

/* MainMenuScript - Controller for main menu functionality
 	Allows player to start the game (or continue.... one day!) */
public class MainMenuScript : MonoBehaviour {

	public static MainMenuScript instance;

	public string firstLevel;

	void Awake() {
		instance = this;	// Allows us to access MainMenuScript from anywhere 
							// (which is ok because there will only ever be one of them active at a time)
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		Debug.Log ("Loading level: " + firstLevel);
		Application.LoadLevel(firstLevel);
	}

	public void ContinueGame() {
		Debug.Log ("Player wants to continue!");
	}
}
