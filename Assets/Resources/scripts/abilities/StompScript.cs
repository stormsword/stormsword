﻿using UnityEngine;
using System.Collections;

public class StompScript : MonoBehaviour {

	/* Components */
	private AbilitySlotScript abilitySlot;	// Get the ability slot that this ability is equipped in
	private CircleCollider2D stompCollider;

	public float cooldown = 2.0f; // Cooldown between abilities
	public float damage = 1;	// Damage the ability does on impact
	public float radius = 5; 	// Radius the ability affects upon impact

	public Transform stompPrefab;

	// Use this for initialization
	void Start () {
		abilitySlot = transform.parent.GetComponent<AbilitySlotScript>();	// Grab the parent ability to get any slot-related info
		stompCollider = GetComponent<CircleCollider2D>();
		stompCollider.radius = radius;	// Collider's radius should always equal effect radius
	}
	
	// Update is called once per frame
	void Update () {
	}

	/* Cast - Trigger the current ability (if off cooldown) */
	internal void Cast() {
		if(CanCast) {
			// Used ability, trigger cooldown
			abilitySlot.Cooldown(cooldown);

			Debug.Log("Cast stomp!");
			var stompTransform = Instantiate(stompPrefab) as Transform;
			stompTransform.transform.parent = transform;	// Stomp effect should be a child of the ability slot (and thus the Character)
			stompTransform.transform.position = transform.position;	// Stomp effect should spawn underneath player
		}
	}

	// Is the ability ready to be cast?
	public bool CanCast {
		get {
			return(abilitySlot.abilityCooldown <= 0f);	
		}
	}
}