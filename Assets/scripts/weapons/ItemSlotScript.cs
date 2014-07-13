using UnityEngine;
using System.Collections;

/* MainHandSlotScript - Controls a main hand weapon or item */

public class ItemSlotScript : MonoBehaviour {

	/* Components */
	private Transform itemPrefab;	// Item that is currently equipped in this slot

	// Remaining cooldown for attack
	public float attackCooldown;


	// Use this for initialization
	void Start () {
		attackCooldown = 0f;	// No item in slot has attacked yet so default to zero

	}
	
	// Update is called once per frame
	void Update () {
		// If slot has a cooldown, slowly decrement the cooldown until it reaches zero
		if(attackCooldown > 0) {
			attackCooldown -= Time.deltaTime;
		}
	}

	// An item in this slot triggered the slot's cooldown
	public void Cooldown(float amount) {
		attackCooldown = amount;
	}
}
