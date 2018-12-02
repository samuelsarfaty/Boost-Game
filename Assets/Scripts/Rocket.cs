using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	[SerializeField] float mainThrust = 0f;
	[SerializeField] float gravityMultiplier = 0f;
	[SerializeField] float torqueFactor = 0f; //TODO used for rotation
	public float landingSpeedThreshold = 0f;
	[SerializeField] float ySnapPosition = 0f;
	[SerializeField] float snapSpeed = 0f;
	[SerializeField] float levelLoadDelay = 0f;

	[SerializeField] AudioClip mainEngineSound = null;
	[SerializeField] AudioClip deathSound = null;

	[SerializeField] ParticleSystem mainEngineParticles = null;
	[SerializeField] ParticleSystem deathparticles = null;

	[HideInInspector] public float speed;

	private Rigidbody rigidBody;
	private AudioSource audioSource;

	private GameObject lastCheckpoint;
	private Animator cameraManager;


	public enum State {Flying, Dying, Grounded}
	public State state = State.Flying;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.centerOfMass = transform.position; //Control rotation by pivot
		audioSource = GetComponent<AudioSource> ();

		cameraManager = GameObject.Find("CameraManager").GetComponent<Animator>();

	}

	void FixedUpdate(){

		speed = rigidBody.velocity.magnitude;

		if(state != State.Dying){
			RespondToInput();
		}
	}

	void OnCollisionEnter(Collision other){

		if (state != State.Flying){ return; }

		switch(other.gameObject.tag)
		{

		case "Friendly":
			break;

		case "Checkpoint":
			if(other.contacts[0].thisCollider.name != "Body" && speed <= landingSpeedThreshold){
				//print (speed);
				Vector3 positionToSnap = new Vector3 (other.transform.position.x, other.transform.position.y + ySnapPosition, 0);
				SnapToCheckpoint (positionToSnap, snapSpeed, snapSpeed);
				SetNextCheckpoint (other.gameObject.GetComponent<Checkpoint> ().nextCameraIndex, other.gameObject);
			} else {
				//print (speed);
				StartDeathSequence ();
				print (rigidBody.velocity.magnitude);
			}
			break;

		default:
			StartDeathSequence ();
			break;
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject.tag == "Checkpoint" && state == State.Grounded){
			state = State.Flying;
		}
	}

	//Checkpoint Update Methods

	public void SnapToCheckpoint(Vector3 position, float posDuration, float rotDuration){
		StartCoroutine (SnapPosition (position, posDuration));
		StartCoroutine (SnapRotation (rotDuration));
	}

	IEnumerator SnapPosition (Vector3 position, float duration){
		rigidBody.isKinematic = true;
		Vector3 currentPos = transform.position;
		float progress = 0f;
		float startTime = Time.time;

		while (progress < 1){
			progress = (Time.time - startTime) / duration;
			transform.position = Vector3.Lerp (currentPos, position, progress);
			yield return null;
		}

		rigidBody.isKinematic = false;
		state = State.Grounded;
	}

	IEnumerator SnapRotation (float duration){
		Quaternion currentRotation = transform.rotation;
		float progress = 0f;
		float startTime = Time.time;

		while (progress < 1){
			progress = (Time.time - startTime) / duration;
			transform.rotation = Quaternion.Lerp (currentRotation, Quaternion.identity, progress);
			yield return null;
		}

		rigidBody.isKinematic = false;
	}

	void SetNextCheckpoint(int index, GameObject currentCheckpoint){
		cameraManager.SetInteger ("cameraIndex", index);
		lastCheckpoint = currentCheckpoint;
	}
		
	//Input Methods
		
	private void RespondToInput(){

		if (Input.GetKey("a")){
			ApplyRotation (Vector3.forward);
		}

		if (Input.GetKey("l")){
			ApplyRotation (-Vector3.forward);
		}

		if (Input.GetKey("a") && Input.GetKey("l")){
			ApplyThrust ();
		} else {
			mainEngineParticles.Stop ();
			audioSource.Stop ();
			rigidBody.AddForce (-Vector3.up * gravityMultiplier, ForceMode.Acceleration); //if player is not thrusting, push rocket down
		}
	}

	private void ApplyRotation(Vector3 direction){
		rigidBody.AddRelativeTorque(direction * torqueFactor, ForceMode.Acceleration);
	}

	private void ApplyThrust (){
		//float thrustThisFrame = mainThrust * Time.deltaTime;
		rigidBody.AddRelativeForce (Vector3.up * mainThrust, ForceMode.Acceleration);

		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot (mainEngineSound);
		}
		mainEngineParticles.Play ();
	}

	//Game Management

	private void StartDeathSequence(){
		state = State.Dying;
		audioSource.Stop ();
		audioSource.PlayOneShot (deathSound);
		deathparticles.Play ();
		Invoke ("LoadLastCheckpoint", levelLoadDelay);
	}

	private void LoadLastCheckpoint(){
		Vector3 lastCheckpointLocation = lastCheckpoint.transform.position;
		transform.position = new Vector3 (lastCheckpointLocation.x, lastCheckpointLocation.y + ySnapPosition, lastCheckpointLocation.z);
		transform.rotation = Quaternion.identity;
		state = State.Grounded;
	}
  



}
