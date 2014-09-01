using UnityEngine;
using System.Collections;

/* DeathScript - Should be enabled when you want to kill the current character */
public class DeathScript : MonoBehaviour {
	
	private MoveScript moveScript;
	private WeaponScript weaponScript;
	private HealthBar healthBar;

	private GameObject rockSlide;

	// Use this for initialization
	void Start () {
		this.enabled = false;

		moveScript = GetComponent<MoveScript>();
		weaponScript = GetComponentInChildren<WeaponScript>();
		healthBar = GetComponentInChildren<HealthBar>();
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(Die());
	}

	// Kill the current game object
	IEnumerator Die() {
		// Stop movement
		moveScript.enabled = false;

		// Stop attacking
		weaponScript.enabled = false;

		// Hide health bar
		healthBar.enabled = false;

		// Play death animation

		// Pause while dying
		yield return new WaitForSeconds(3);

		// Destroy enemy
		GameObject.Destroy(gameObject);

		// Open rockslide door
		rockSlide = GameObject.Find ("Rockslide(Clone)");	// I'm not sure why we need to add '(Clone)' but it doesn't work without it.
		GameObject.Destroy(rockSlide);
	}
}
