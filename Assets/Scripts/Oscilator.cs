using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour {

	[SerializeField] Vector3 movementVector = Vector3.zero;
	[SerializeField] float period = 0f;

	[Range(0,1)]
	[SerializeField]
	float movementFactor;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (period <= Mathf.Epsilon) {
			return;
		}
			
		float cycles = Time.time / period;

		const float tau = Mathf.PI * 2;
		float rawSineWave = Mathf.Sin (cycles * tau);
		movementFactor = rawSineWave / 2f + 0.5f;

		Vector3 offset = movementVector * movementFactor;
		transform.position = startPos + offset;
	}
}
