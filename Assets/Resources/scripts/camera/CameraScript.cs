using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	private Transform target;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(target) {
			// Only track if player is still alive
			Vector3 point = camera.WorldToViewportPoint(target.position);	// Player's current position
			
			float cameraMovementX = 0.5f;
			float cameraMovementY = 0.5f;
			
			if ((target.position.x - 3f) < 0) {
				cameraMovementX = point.x;
				Debug.Log (cameraMovementX);
			}
			
			if ((target.position.y - 2.8f) < 0) {
				cameraMovementY = point.y;
			}
			
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(cameraMovementX, cameraMovementY, point.z));
			Vector3 destination = transform.position + delta;
			this.transform.position = Vector3.SmoothDamp (this.transform.position, destination, ref velocity, dampTime);
		}
	}
}