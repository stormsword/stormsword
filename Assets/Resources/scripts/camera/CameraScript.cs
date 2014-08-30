using UnityEngine;
using System.Collections;


/* CameraScript - Controls camera movement within a scene

	Camera Modes:
	FollowPlayer - Sticks to the player and follows him/her around the map
	Scroll - Pans in a semi-cinematic style around the map while a menu (i.e. main menu) is displayed
	Goto - Pans to a specific point on the map
 */
public class CameraScript : MonoBehaviour {

	[System.Serializable]
	public enum CameraModes {
		FollowPlayer,
		Scroll,
		Goto
	}

	public CameraModes cameraMode;

	public float followSpeed = 0.13f;
	public float scrollSpeed = 0.2f;
	public float gotoSpeed = 1.0f;

	private float speed;

	private Vector3 velocity = Vector3.zero;
	private Transform target;
	private Vector2 destination = Vector2.zero;

	// Use this for initialization
	void Start () {
		// By default we follow the player around the map unless 
		if(cameraMode == CameraModes.FollowPlayer) {
			speed = followSpeed;
			target = GameObject.FindGameObjectWithTag("Player").transform;
			this.transform.position = target.position;	// Snap to player's position when the game starts
		} else if (cameraMode == CameraModes.Goto) {
			speed = gotoSpeed;
		} else {
			speed = scrollSpeed;
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
		else if(cameraMode == CameraModes.Scroll) {
			// Slowly pan to the right in a semi-cinematic fashion
			Vector3 delta = new Vector3(1, 0);
			Vector3 destination = transform.position + delta;
			this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, speed);
		}
		else if(cameraMode == CameraModes.Goto) {
			// Pan to a specific point on the map (usually used to reveal things to the player)
			this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, speed);
		}
	}
	
	/* Goto - Set a destination and head there */
	public void Goto(Vector2 _destination) {
		speed = gotoSpeed;
		destination = _destination;
		cameraMode = CameraModes.Goto;
	}	
}