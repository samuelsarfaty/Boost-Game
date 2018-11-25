using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour {

	[SerializeField] int nextCameraIndex = 0;
	private static Animator cameraManager;
	private static Rocket player;

	void Awake(){
		if (!cameraManager){
			cameraManager = GameObject.Find ("CameraManager").GetComponent<Animator> ();
		}

		if (!player){
			player = GameObject.FindObjectOfType<Rocket> ();
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject == player.gameObject && player.state != Rocket.State.Dying){
			SetVirtualCameraIndex (nextCameraIndex);
			GameManager.lastCheckpoint = this;
		}
	}

	void SetVirtualCameraIndex (int index){
		cameraManager.SetInteger ("cameraIndex", index);
	}
}
