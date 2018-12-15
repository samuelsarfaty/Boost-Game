using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour {

	public Transform target;

	void FixedUpdate(){
		transform.LookAt (target);
	}
}
