using UnityEngine;
using System.Collections;

/* HealthScript - Gives an object HP */

public class HealthScript : MonoBehaviour {

	// Total Hitpoints
	public int hp = 1;

	// Enemy or Player?
	public bool isEnemy = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/* Damage - Inflicts damage and check if the object should be destroyed */
	public void Damage(int damageCount) {
		hp -= damageCount;

		if (hp <= 0) {
			// Object is dead
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider) {

		// Is this a shot?
		ProjectileScript shot = otherCollider.gameObject.GetComponent<ProjectileScript>();

		if (shot != null) {
			// Ignore friendly fire
			if(shot.isEnemyShot != isEnemy) {
				Damage(shot.damage);

				// Destroy the shot
				Destroy(shot.gameObject);

			}
		}
	}
}
