using UnityEngine;
using System.Collections;

/* DeathScript - Should be enabled when you want to kill the current character */
public class DeathScript : MonoBehaviour {
	
	private MoveScript moveScript;
	private WeaponScript weaponScript;

	// Use this for initialization
	void Start () {
		this.enabled = false;

		// Stop movement
		moveScript = GetComponent<MoveScript>();

		// Stop attacking
		weaponScript = GetComponentInChildren<WeaponScript>();

	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(Die());
	}

	// Kill the current game object
	IEnumerator Die() {
		moveScript.enabled = false;
		weaponScript.enabled = false;

		// Play death animation

		// Pause while dying
		yield return new WaitForSeconds(3);

		// Destroy enemy
		GameObject.Destroy(gameObject);
	}
}
