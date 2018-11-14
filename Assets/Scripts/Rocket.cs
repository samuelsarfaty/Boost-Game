using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

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

	enum State {Alive, Dying, Flying}
	State state = State.Alive;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.centerOfMass = transform.position; //Control rotation by pivot
		audioSource = GetComponent<AudioSource> ();
	}

	void FixedUpdate(){
		if(state == State.Alive){
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
		
	private void RespondToInput(){

		if (Input.GetKey("a")){
			ApplyRotation (Vector3.forward);
		}

		if (Input.GetKey("d")){
			ApplyRotation (-Vector3.forward);
		}

		if (Input.GetKey("a") && Input.GetKey("d")){
			ApplyThrust ();
		} else {
			mainEngineParticles.Stop ();
			audioSource.Stop ();
		}
	}

	private void ApplyRotation(Vector3 direction){
		rigidBody.AddRelativeTorque(direction * torqueFactor, ForceMode.Acceleration);
	}

	private void ApplyThrust (){
		float thrustThisFrame = mainThrust * Time.deltaTime;
		rigidBody.AddRelativeForce (Vector3.up * thrustThisFrame, ForceMode.Acceleration);

		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot (mainEngineSound);
		}
		mainEngineParticles.Play ();
	}

	private void StartWinSequence(){
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

}
