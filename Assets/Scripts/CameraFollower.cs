using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

	Rocket player;
	Vector3 offset;

	void Awake(){
		player = GameObject.FindObjectOfType<Rocket> ();
		offset = transform.position - player.transform.position;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position + offset;
	}
}
