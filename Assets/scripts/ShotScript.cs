using UnityEngine;
using System.Collections;

/* ShotScript - Used for shooting projectiles across the screen */

public class ShotScript : MonoBehaviour {

	// Damage the object deals
	public int damage = 1;

	// Should the projectile damage players? or enemies?
	public bool isEnemyShot = false;
	
	void Start () {
		Destroy(gameObject, 20);	// Only lives for 20 seconds to prevent 'leaks'
	}

	void Update () {
	
	}
}
