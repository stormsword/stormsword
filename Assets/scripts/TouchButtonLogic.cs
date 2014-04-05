using UnityEngine;
using System.Collections;

public class TouchButtonLogic : MonoBehaviour {
	void Update() {
		// Is there a touch on screen?
		if(Input.touchCount > 0) {
			// At least one touch event is present

			for(int i = 0; i < Input.touchCount; i++) {
				var touch = Input.GetTouch(i);
				if(this.guiTexture.HitTest(touch.position)) {
					// Touch is within our texture

					if(touch.phase == TouchPhase.Began) {
						Debug.Log("The touch is begun on " + this.name);
					}
					if(touch.phase == TouchPhase.Ended) {
						Debug.Log ("The touch has ended on " + this.name);
					}
				}
			}

			// Loop through the active touches
		}
		else {
			// No active touches
		}

	}
}