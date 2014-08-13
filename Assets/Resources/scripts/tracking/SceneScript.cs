using UnityEngine;
using System.Collections;

/* SceneScript - Called when a Scene is loaded */
public class SceneScript : MonoBehaviour {

	private TiledMapComponent mapComponent;

	// Scene has loaded, send data so we know how far a player gets
	void Start () {
		mapComponent = GetComponent<TiledMapComponent>();
		if(mapComponent) {
			TrackingScript.instance.AddProperty("scene", mapComponent.MapTMX.name);
			TrackingScript.instance.Track("scene loaded");
		}
	}
}
