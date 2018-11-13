using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float rotationThrust = 0;
	[SerializeField] float mainThrust = 0;
	[SerializeField] float levelLoadDelay = 0;
	[SerializeField] float torqueFactor = 0; //TODO used for rotation
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
		rigidBody.centerOfMass = transform.position; //Control rotation by pivot
		audioSource = GetComponent<AudioSource> ();
		colliders = GetComponentsInChildren<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*if(state == State.Alive){
			//RespondToThrust ();
			//RespondToRotation ();
			RespondToInput();
		}*/

		if (Debug.isDebugBuild){ //Debug keys enabled only on development mode
			if (Input.GetKeyDown("c")){
				ToggleColliders ();
			}
		}
			
	}

	void FixedUpdate(){
		if(state == State.Alive){
			//RespondToThrust ();
			//RespondToRotation ();
			RespondToInput();
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
			/*rigidBody.freezeRotation = true; //Take control over rotation
			transform.Rotate (Vector3.forward * rotationThisFrame);
			rigidBody.freezeRotation = false; //Take control over rotation*/

			rigidBody.AddTorque (Vector3.forward * torqueFactor);
		} 

		else if (Input.GetKey(KeyCode.D)){
			//Rotate right
			/*rigidBody.freezeRotation = true; //Take control over rotation
			transform.Rotate (-Vector3.forward * rotationThisFrame);
			rigidBody.freezeRotation = false; //Take control over rotation*/

			rigidBody.AddTorque (-Vector3.forward * torqueFactor);
		}
	}

	private void ApplyRotation(Vector3 direction){
		rigidBody.AddTorque (direction * torqueFactor);

	}

	private void RespondToInput(){

		if (Input.GetKey("a")){
			ApplyRotation (Vector3.forward);
		}

		if (Input.GetKey("d")){
			ApplyRotation (-Vector3.forward);
		}

		if (Input.GetKey("a") && Input.GetKey("d")){
			ApplyThrust ();
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
