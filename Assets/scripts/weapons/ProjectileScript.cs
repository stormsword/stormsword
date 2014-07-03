using UnityEngine;
using System.Collections;

/* ProjectileScript - Used for shooting projectiles across the screen */

public class ProjectileScript : MonoBehaviour {

	// Damage the object deals
	public int damage = 1;
	public float duration = 0.25f;

	// Should the projectile damage players? or enemies?
	public bool isEnemyShot = false;
	
	void Start () {
		Destroy(gameObject, duration);	// Only lives for 20 seconds to prevent leaks
	}

	void Update () {
	
	}
}
