using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour {

	[SerializeField] CinemachineVirtualCamera virtualCamera;
	[SerializeField] float dollyTargetPosition;
	private CinemachineTrackedDolly trackedDolly;

	void Awake(){
		trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly> ();
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player"){
			FixCamera (dollyTargetPosition);
		}
	}

	void FixCamera(float targetPosition){
		transform.rotation = Quaternion.identity;
		trackedDolly.m_AutoDolly.m_Enabled = false;
		virtualCamera.Follow = null;
		virtualCamera.LookAt = null;
		trackedDolly.m_PathPosition = targetPosition;

	}
}
