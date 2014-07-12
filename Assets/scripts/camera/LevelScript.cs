using UnityEngine;
using System.Collections;
using SpriteTile;

/* LevelScript - Use to load scenes. Loads the current level, loads tilemaps, sets up camera */

public class LevelScript : MonoBehaviour {

	public TextAsset grassLevel;

	void Awake() {
		Tile.SetCamera();
		Tile.LoadLevel(grassLevel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
