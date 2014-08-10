using UnityEngine;
using System.Collections;

/* SceneScript - Called when a Scene is loaded */
public class SceneScript : MonoBehaviour {

	private TiledMapComponent mapComponent;

	// Use this for initialization
	void Start () {
		mapComponent = GetComponent<TiledMapComponent>();
		if(mapComponent) {
			TrackingScript.instance.AddProperty("scene", mapComponent.MapTMX.name);
			TrackingScript.instance.Track("scene loaded");
		}
	
	}
}
