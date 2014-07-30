﻿using UnityEngine;
using System.Collections;

/* AbilityUIScript - places ability UI elements on the screen */
public class AbilityUIScript : MonoBehaviour {

	public int x;					// X coordinate to display the image at
	public int y;					// Y coordinate to display the image at
	public int width;				// Width of the image
	public int height;				// Height of the image
	
	public Texture2D abilityImage;	// The image to display for this ability

	public float fadeAlpha = 0.6f;

	private float currentAlpha = 1f;		// Current Alpha (opacity) value of the image


	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI() {

		// Draw a frame around the ability


		// Draw the ability's image and apply opacity to fade if necessary
		Color tmpColor = GUI.color;		// Placeholder for current color setting
		GUI.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, currentAlpha);
		GUI.Button(new Rect(x, y, width, height), abilityImage);	// Draw the actual image
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
