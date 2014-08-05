using UnityEngine;
using System.Collections;

/* hpBubbleScript - Spawns an hp bubble above the player's head when damage is taken and floats it away */

public class hpBubbleScript : MonoBehaviour {

	/* Components */
	private CharacterScript character;	// Character taking dmg
	private HealthScript healthScript;	// Health of character taking dmg
	
	public float duration = 20f;	// Duration bubble appears on screen
	public Color color = new Color(0.8f, 0.8f, 0.0f, 1.0f);
	public float scroll = 0.05f;  // scrolling velocity
	public float alpha = 1;

	// Is attack dmg or a heal? (Passed in by Healthscript)
	public dmgTypes dmgType;
	public float amount;

	void Start () {

		// Damage should have a '-' in front of it, heals a '+'
		switch(dmgType) {
		case dmgTypes.Damage:
			// Set actual dmg number
			guiText.text = "-" + amount.ToString();
			break;
		case dmgTypes.Heal:
			// Set actual heal number
			guiText.text = "+" + amount.ToString();
			break;
		}

		// Setup visual styling
		guiText.material.color = color; // set text color

		character = transform.parent.GetComponent<CharacterScript>();
		if(character) {
			// spawn health bubble where character taking dmg starts
			transform.position = Camera.main.WorldToViewportPoint(character.transform.position);
		}
	
		Destroy(gameObject, duration);	// Only shows up for a short period of time

	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector2(transform.position.x, transform.position.y + scroll*Time.deltaTime); 

		// Adjust alpha transparency of hp bubble to slowly fade it out
		alpha -= Time.deltaTime/duration;
		Color tmpColor = guiText.material.color;
		tmpColor.a = alpha;
		guiText.material.color = tmpColor;
	}
}
