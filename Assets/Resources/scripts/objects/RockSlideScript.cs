using UnityEngine;
using System.Collections;

public class RockSlideScript : MonoBehaviour {

	[Tooltip("The Game Object to spawn when the player enters the trigger")]
	public Transform RockSlide;

	private GameObject rockSlide;

	/* SpawnSlide - Spawns the rock slide object */
	public void SpawnSlide() {
		rockSlide = Instantiate(RockSlide) as GameObject;
	}

	public void DestroySlide() {
		GameObject.Destroy(rockSlide);
	}

}
