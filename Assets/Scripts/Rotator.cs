using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] bool rotateAroundObject = false;
	[Tooltip("Add target if you want to rotate around object")]
	[SerializeField] Transform target = null;
	//[SerializeField] Vector3 rotationVector = Vector3.zero;
	[SerializeField] Vector3 rotationAxis = Vector3.zero;
	[SerializeField] float speed = 0f;
	
	// Update is called once per frame
	void Update () {

		if(rotateAroundObject){
			RotateAround (target.position, rotationAxis, speed);
		} else {
			Rotate ();	
		}
	}

	void Rotate(){
		transform.Rotate (rotationAxis, speed * Time.deltaTime);
	}

	void RotateAround(Vector3 target, Vector3 axis, float speed){
		transform.RotateAround (target, axis, speed * Time.deltaTime);
	}
}
