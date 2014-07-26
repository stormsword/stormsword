using UnityEngine;
using System.Collections;

public class EffectScript : MonoBehaviour {

	public float duration = 0.5f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, duration);	// effect should go away after <duration>
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
