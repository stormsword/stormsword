using UnityEngine;
using System.Collections;

/* AbilityUIScript - places ability UI elements on the screen */
public class AbilityUIScript : MonoBehaviour {

	public int x;					// X coordinate to display the image at
	public int y;					// Y coordinate to display the image at
	public int width;				// Width of the image
	public int height;				// Height of the image

	public float fadeAlpha = 0.6f;

	private float currentAlpha = 1f;		// Current Alpha (opacity) value of the image

	// UI Position
	internal int abilityIndex = 0;			// What # in the array is this ability?

	// Ability Hotkey
	private GameObject player;				// Player manning the keyboard
	private PlayerScript playerScript;		// Script with player input code to grab key bindings
	private string abilityKey;						// Key used to trigger abilities

	// Ability Icon
	private AbilitySlotScript abilitySlotScript;	// Current ability slot
	private AbilityScript abilityScript;		// Currently equipped ability (to get the icon)
	private Transform ability;
	private Texture2D abilityIcon;	// The image to display for this ability


	// Use this for initialization
	void Start () {
		// Get ability hotkey
		GameObject player = GameObject.FindWithTag("Player");
		playerScript = player.GetComponent<PlayerScript>();
		abilityKey = playerScript.ability1Key.ToString();

		// Get ability icon
		abilitySlotScript = GetComponent<AbilitySlotScript>();
		ability = abilitySlotScript.abilityEquipped;		// Currently equipped ability
		abilityScript = ability.gameObject.GetComponent<AbilityScript>();
		abilityIcon = abilityScript.abilityIcon;
	}
	
	void OnGUI() {

		// If there are are multiple abilities equipped, show them next to each other, not on top of each other
		int positioning_x = abilityIndex * 40;	
		int positioning_y = 0;

		// Draw the ability's image and apply opacity to fade if necessary
		Color tmpColor = GUI.color;		// Placeholder for current color setting
		GUI.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, currentAlpha);
		GUI.Button(new Rect(x + positioning_x, y + positioning_y, width, height), abilityIcon);	// Draw the actual image
		GUI.Label (new Rect(x-1 + positioning_x, y+15 + positioning_y, width, height), abilityKey);
		GUI.color = tmpColor;			// Restore color setting for other GUI elements
	}

	/* FadeOut - Called to fade the current UI element to partial opacity
	 	e.g. when it's not available */
	internal void FadeOut() {
		currentAlpha = fadeAlpha;
	}

	/* FadeIn - Called to fade the current UI element to full opacity */
	internal void FadeIn() {
		currentAlpha = 1f;	// 1f = 'fully visible'
	}
}
