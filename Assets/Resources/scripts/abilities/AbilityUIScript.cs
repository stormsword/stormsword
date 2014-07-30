using UnityEngine;
using System.Collections;

/* AbilityUIScript - places ability UI elements on the screen */
public class AbilityUIScript : MonoBehaviour {

	public int x;					// X coordinate to display the image at
	public int y;					// Y coordinate to display the image at
	public int width;				// Width of the image
	public int height;				// Height of the image


	public Texture2D abilityImage;	// The image to display for this ability

	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI() {
		GUI.Label(new Rect(x, y, width, height), abilityImage);
	}
}
