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
		if (other.gameObject == player.gameObject){
			SetVirtualCameraIndex (nextCameraIndex);
			//LocatePlayer ();
		}
	}

	void SetVirtualCameraIndex (int index){
		cameraManager.SetInteger ("cameraIndex", index);
	}

	/*void LocatePlayer(){
		if (player.state == Rocket.State.Flying){
			Vector3 positionToSnap = new Vector3 (transform.position.x, transform.position.y + playerSnapYPosition, 0);
			player.SnapToCheckpoint (positionToSnap, player.snapSpeed, player.snapSpeed);

		}
	}*/
}
