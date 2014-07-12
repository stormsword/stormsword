using UnityEngine;
using System.Collections;
using SpriteTile;

/* LevelScript - Use to load scenes. Loads the current level, loads tilemaps, sets up camera */

public class LevelScript : MonoBehaviour {

	public TextAsset grassLevel;

	void Awake() {
		Tile.SetCamera();	// By default, uses the camera tagged 'MainCamera'
		Tile.LoadLevel(grassLevel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
