using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour {

	[SerializeField] int nextCameraIndex = 0;
	private CinemachineBrain cmBrain;

	void Awake(){
		cmBrain = Camera.main.GetComponent<CinemachineBrain> ();
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player"){
			SetVirtualCameraIndex (nextCameraIndex);
		}
	}

	void SetVirtualCameraIndex (int index){
		
	}
}
