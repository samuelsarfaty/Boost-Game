using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] Vector3 rotationVector = Vector3.zero;
	[SerializeField] float speed = 0f;
	
	// Update is called once per frame
	void Update () {
		Rotate ();
	}

	void Rotate(){
		transform.Rotate (rotationVector, speed * Time.deltaTime);
	}
}
