using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour {

	public Vector3 movementDirection;
	public float speed;

	private Rocket player;
	private bool wasActivated;
	private Vector3 startPos;

	void Awake(){
		player = GameObject.FindObjectOfType<Rocket> ();
		wasActivated = false;
		startPos = transform.position;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player" && !wasActivated){
			wasActivated = true;
			print ("run!");
		}	
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Stop"){
			Stop ();
		}
	}

	void Update(){
		if(wasActivated){
			Move ();
		}

		if (player.state == Rocket.State.Dying){
			Invoke ("Reset", player.levelLoadDelay);
		}
	}

	void Move(){
		transform.position += movementDirection * speed * Time.deltaTime;
	}

	void Stop(){
		wasActivated = false;
	}

	void Reset(){
		transform.position = startPos;
		wasActivated = false;
	}
}
