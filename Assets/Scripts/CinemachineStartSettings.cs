using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineStartSettings : MonoBehaviour {

	[SerializeField] Transform followTarget = null;
	[SerializeField] Transform lookAtTarget = null;

	[Tooltip("If AutoDolly is enabled, value will default to 0 ")]
	[SerializeField] float startPathPosition = 0f;
	[SerializeField] bool autoDollyEnabled = true;

	private CinemachineVirtualCamera virtualCamera;

	void Awake(){
		virtualCamera = GetComponent<CinemachineVirtualCamera> ();
	}

	void Start(){
		virtualCamera.Follow = followTarget;
		virtualCamera.LookAt = lookAtTarget;
		virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly> ().m_PathPosition = startPathPosition;
		virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly> ().m_AutoDolly.m_Enabled = autoDollyEnabled;
	}
}
