using UnityEngine;
using System.Collections;

public enum CameraModes {
	FollowPlayer,
	Menu
}

/* CameraScript - Controls camera movement within a scene

	Camera Modes:
	FollowPlayer - Sticks to the player and follows him/her around the map
	Menu - Pans in a semi-cinematic style around the map while a menu (i.e. main menu) is displayed
 */
public class CameraScript : MonoBehaviour {

	public CameraModes cameraMode;
	public float speed = 0.15f;
	private Vector3 velocity = Vector3.zero;
	private Transform target;

	// Use this for initialization
	void Start () {
		if(cameraMode == CameraModes.FollowPlayer) {
			target = GameObject.FindGameObjectWithTag("Player").transform;
			this.transform.position = target.position;	// Snap to player's position when the game starts
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(target && (cameraMode == CameraModes.FollowPlayer)) {
			// Only track if player is still alive
			Vector3 point = camera.WorldToViewportPoint(target.position);	// Player's current position
			
			float cameraMovementX = 0.5f;
			float cameraMovementY = 0.5f;

			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(cameraMovementX, cameraMovementY, point.z));
			Vector3 destination = transform.position + delta;
			this.transform.position = Vector3.SmoothDamp (this.transform.position, destination, ref velocity, speed);
		}
		else if(cameraMode == CameraModes.Menu) {
			// Slowly pan to the right in a semi-cinematic fashion
			Vector3 delta = new Vector3(1, 0);
			Vector3 destination = transform.position + delta;
			this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, speed);
		}
	}
}