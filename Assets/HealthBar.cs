using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	/* Components */
	private CharacterScript character;	// Character to display HP for
	private HealthScript health;		// Health of above character

	private int maxHp = 0;
	private int currentHp = 0;
	private string characterName = "";

	// Use this for initialization
	void Start () {
		// Find character by searching parent's character script
		character = transform.parent.GetComponent<CharacterScript>();
		if(character) {
			// Assuming character exists)
			health = character.GetComponent<HealthScript>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentHp = health.hp;

		// Lock the hp bar to the player's head
		transform.position = Camera.main.WorldToViewportPoint(character.transform.position);
	}
}
