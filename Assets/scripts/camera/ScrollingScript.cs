using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* ScrollingScript - Used to scroll a background or static object 

 Usage: Assign to a layer you want to scroll

 isLinkedToCamera (bool) - Does the current object also move the camera? */

public class ScrollingScript : MonoBehaviour {

	public Vector2 speed = new Vector2(2, 2);
	public Vector2 direction = new Vector2(-1, 0);

	public bool isLinkedToCamera = false;

	// Should the background be infinite?
	public bool isLooping = false;

	// List of children with a renderer
	private List<Transform> backgroundPart;

	void Start() {

		// Get all of the children in this layer
		if(isLooping) {

			backgroundPart = new List<Transform>();
			
			for(int i=0; i< transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				
				// Add only the visible children
				if(child.renderer != null) {
					backgroundPart.Add(child);
				}
			}
			
			// Sort by position
			// Get the children from left to right (need to add more conditions to handle scrolling in all directions)
			backgroundPart = backgroundPart.OrderBy (
				t => t.position.x
				).ToList();
		}
	}

	// Update is called once per frame
	void Update () {

		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

		// Move the Camera
		if(isLinkedToCamera) {
			Camera.main.transform.Translate(movement);
		}

		// Handle Looping backgrounds
		if(isLooping) {
			// Get the first object
			// The list is ordered from left (x position) to right
			Transform firstChild = backgroundPart.FirstOrDefault();

			if(firstChild != null) {
				// Check if child is already (partly) before the camera
				if(firstChild.position.x < Camera.main.transform.position.x) {
					// If the child is already on the left, test if it's completely outside and should be recycled
					if(firstChild.renderer.IsVisibleFrom(Camera.main) == false) {
						// Get the last child position
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);

						// Set the position of the recycled piece to be AFTER the last child (only for horizontal scrolls)
						firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

						// Set the recycled child to the last position of the backgroundPart list
						backgroundPart.Remove(firstChild);
						backgroundPart.Add (firstChild);
					}
				}
			}
		}
	}
}
