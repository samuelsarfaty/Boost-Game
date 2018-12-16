using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSkipper : MonoBehaviour {

	public Transform[] checkpoints;
	public Rocket player;

	private int selectedCheckpoint = 0;

	public void NextCheckpoint(){
		if(selectedCheckpoint < checkpoints.Length - 1){
			selectedCheckpoint++;
			Vector3 nextPos = new Vector3 (checkpoints [selectedCheckpoint].position.x, checkpoints [selectedCheckpoint].position.y + player.ySnapPosition, 
								checkpoints[selectedCheckpoint].position.z);
			
			player.transform.position = nextPos;
		}
	}

	public void PreviousCheckpoint(){

		if(selectedCheckpoint > 0){
			selectedCheckpoint--;
			Vector3 nextPos = new Vector3 (checkpoints [selectedCheckpoint].position.x, checkpoints [selectedCheckpoint].position.y + player.ySnapPosition, 
								checkpoints[selectedCheckpoint].position.z);
			
			player.transform.position = nextPos;
		}
	}
}
