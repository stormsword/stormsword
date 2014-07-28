using UnityEngine;
using System.Collections;

/* Effect - Extend this to create a buff/debuff that affects a character 
 e.g. if you want to reduce a character's movement speed by 20% or apply a poison
 */

public class Effect : MonoBehaviour {

	public float duration;		// How long (seconds) should the effect last
	public float startTime;		// How long (seconds) until the effect starts
	public float tick;			// How long (seconds) between each pulse of the effect

	public GameObject target;	// The target the effect is affecting
	
	void Start () {
		// Apply the affect
		InvokeRepeating("ApplyEffect", startTime, tick);	// Apply the affect every 'tick' seconds

		Invoke ("EndEffect", duration);			// In 'duration' seconds, destroy the effect
	}

	protected virtual void ApplyEffect() {
		// Extend this function to add additional logic upon application (i.e. set movespeed -20%)
	}

	protected virtual void EndEffect() {
		// Extend this function to add additional logic upon removal (i.e. return movespeed to 100%)

		CancelInvoke();			// Stop any existing functions
		Destroy(gameObject);	// Destroy the game object (removing the affect)
	}
}
