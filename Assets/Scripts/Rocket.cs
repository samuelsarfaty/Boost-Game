using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rotationThrust = 0;
	[SerializeField] float mainThrust = 0;
	[SerializeField] float levelLoadDelay = 0;
	[SerializeField] AudioClip mainEngineSound = null;
	[SerializeField] AudioClip deathSound = null;
	[SerializeField] AudioClip successSound = null;

	[SerializeField] ParticleSystem mainEngineParticles = null;
	[SerializeField] ParticleSystem deathparticles = null;
	[SerializeField] ParticleSystem successParticles = null;

	Rigidbody rigidBody;
	AudioSource audioSource;
	Collider[] colliders;

	enum State {Alive, Dying, Transcending}
	State state = State.Alive;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		colliders = GetComponentsInChildren<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.Alive){
			RespondToThrust ();
			RespondToRotation ();
		}

		if (Debug.isDebugBuild){ //Debug keys enabled only on development mode
			if (Input.GetKeyDown("c")){
				ToggleColliders ();
			}
		}
	}

	void OnCollisionEnter(Collision collision){

		if (state != State.Alive){ return; }

		switch(collision.gameObject.tag)
		{
		case ("Friendly"):
			print ("OK");
			break;

		case ("Finish"):
			StartWinSequence ();
			break;

		default:
			StartDeathSequence ();
			break;
		}
	}

	private void LoadNextScene(){
		int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;

		if(nextSceneIndex > SceneManager.sceneCountInBuildSettings){
			nextSceneIndex = 0;
		}
			
		SceneManager.LoadScene (nextSceneIndex);
	}

	private void RestartScene(){
		SceneManager.LoadScene (0);
	}

	private void RespondToThrust (){
		if (Input.GetKey (KeyCode.Space)) {
			ApplyThrust ();
		}
		else {
			audioSource.Stop ();
			mainEngineParticles.Stop ();
		}
	}

	private void RespondToRotation(){


		float rotationThisFrame = rotationThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.A)){
			//Rotate left
			rigidBody.freezeRotation = true; //Take control over rotation
			transform.Rotate (Vector3.forward * rotationThisFrame);
			rigidBody.freezeRotation = false; //Take control over rotation
		} 

		else if (Input.GetKey(KeyCode.D)){
			//Rotate right
			rigidBody.freezeRotation = true; //Take control over rotation
			transform.Rotate (-Vector3.forward * rotationThisFrame);
			rigidBody.freezeRotation = false; //Take control over rotation
		}
	}

	private void ApplyThrust (){
		float thrustThisFrame = mainThrust * Time.deltaTime;
		rigidBody.AddRelativeForce (Vector3.up * thrustThisFrame);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot (mainEngineSound);
		}
		mainEngineParticles.Play ();
	}

	private void StartWinSequence(){
		state = State.Transcending;
		audioSource.Stop ();
		audioSource.PlayOneShot (successSound);
		successParticles.Play ();
		Invoke ("LoadNextScene", levelLoadDelay);
	}

	private void StartDeathSequence(){
		state = State.Dying;
		audioSource.Stop ();
		audioSource.PlayOneShot (deathSound);
		deathparticles.Play ();
		Invoke ("RestartScene", levelLoadDelay);
	}

	private void ToggleColliders(){
		foreach (Collider collider in colliders){
			//bool state = collider.enabled;
			collider.enabled = !collider.enabled;
		}
	}


}
