using UnityEngine;
using System.Collections;

public class HealthBarNameScript : MonoBehaviour {

	/* Components */
	internal HealthBar healthBar;	// Character to display HP for
	
	private CharacterScript character;
	private GUIText guitext;


	// Use this for initialization
	void Start () {
		healthBar = transform.parent.GetComponent<HealthBar>();	// Get reference to parent game object

		guitext = GetComponent<GUIText>();	// The text component we need to update
	}
	
	// Update is called once per frame
	void Update () {
		if(healthBar) {
			// This feels like a hack. If you put it in Start() it doesn't load properly.
			character = healthBar.character;	// Get reference to character the healthbar is attached to
		}

		if(character) {
			guitext.text = character.name;
		}
	}
}
