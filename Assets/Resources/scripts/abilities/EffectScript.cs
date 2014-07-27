using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour {

	public float duration = 0.5f;
	private float radius = 0f;

	private CircleCollider2D stompCollider;	// Collider on the stomp effect

	private StompScript stompScript;

	// Use this for initialization
	void Start () {
		stompCollider = GetComponent<CircleCollider2D>();
		stompScript = transform.parent.GetComponent<StompScript>();

		stompCollider.radius = stompScript.radius;	// Stomp effect should always match radius of Stomp

		Destroy (gameObject, duration);	// effect should go away after <duration>
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
