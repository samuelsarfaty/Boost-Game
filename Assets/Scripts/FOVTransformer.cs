using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class FOVTransformer : MonoBehaviour {

	public float targetFOV;

	private float startFOV;
	private CinemachineVirtualCamera virtualCamera;
	private CinemachineTrackedDolly trackedDolly;

	void Awake(){
		virtualCamera = GetComponent<CinemachineVirtualCamera> ();
		trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly> ();
		startFOV = virtualCamera.m_Lens.FieldOfView;
	}

	void Update(){

		if(!trackedDolly){ return; }

		float currentDollyPos = trackedDolly.m_PathPosition;

		float FOVStep = currentDollyPos * (targetFOV - startFOV) + startFOV;

		virtualCamera.m_Lens.FieldOfView = FOVStep;


	}


}
