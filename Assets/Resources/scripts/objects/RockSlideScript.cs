using UnityEngine;
using System.Collections;

public class RockSlideScript : MonoBehaviour {

	[Tooltip("The Game Object to spawn when the player enters the trigger")]
	public Transform RockSlide;

	/* SpawnSlide - Spawns the rock slide object */
	public void SpawnSlide() {
		GameObject rockslide = Instantiate(RockSlide) as GameObject;
	}

}
