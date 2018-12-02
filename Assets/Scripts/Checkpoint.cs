using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour {

	public int nextCameraIndex;
	public Material[] mats;

	private static Rocket player;
	private Renderer myRend;

	void Awake(){
		if (!player){
			player = GameObject.FindObjectOfType<Rocket> ();
		}

		myRend = GetComponent<Renderer> ();
	}

	void Update(){
		if(player.speed > player.landingSpeedThreshold){
			myRend.material = mats [1];
		} else {
			myRend.material = mats [0];
		}
	}



}
