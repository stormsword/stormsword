using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	
	/* Components */
	internal CharacterScript character;	// Character to display HP for
	
	private HealthScript health;		// Health of above character
	private GUITexture currentHPBar;	// Remaining HP bar
	private float currentHPBarStart;	

	// HP Bar Visual Design
	public int BarWidth = 40;
	public int BarHeight = 15;

	// Character Stats
	private int maxHp = 0;
	private int currentHp = 0;



	// Use this for initialization
	void Start () {
		// Find character by searching parent's character script
		character = transform.parent.GetComponent<CharacterScript>();
		if(character) {
			// Assuming character exists)
			health = character.GetComponent<HealthScript>();

			maxHp = health.hp;	// HP when mob spawns is assumed to be the total hp it can have
		}

		// Find current HP bar
		currentHPBar = transform.GetComponentInChildren<GUITexture>();
		currentHPBar.transform.localScale = Vector3.zero;	// Zero-out the scale vector to allow us to manipulate it later
	
	}
	
	// Update is called once per frame
	void Update () {
		currentHp = health.hp;

		// Lock the hp bar to the player's head
		transform.position = Camera.main.WorldToViewportPoint(character.transform.position);

		// Update health bar size when character is damanged
		float HPRatio = (float) decimal.Divide(currentHp, maxHp);	// Dividing two ints normally returns an int - this way we get a float
		float CurrentHPBarWidth = HPRatio * BarWidth;
		Rect newRect = new Rect(10, 10, CurrentHPBarWidth, BarHeight);
		currentHPBar.pixelInset = newRect;
	}

}
