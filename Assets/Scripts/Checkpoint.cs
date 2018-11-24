using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour {

	[SerializeField] int nextCameraIndex = 0;
	private static Animator cameraManager;

	void Awake(){
		if (!cameraManager){
			cameraManager = GameObject.Find ("CameraManager").GetComponent<Animator> ();
		}
	}



	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player"){
			SetVirtualCameraIndex (nextCameraIndex);
		}
	}

	void SetVirtualCameraIndex (int index){
		cameraManager.SetInteger ("cameraIndex", index);
	}
}
