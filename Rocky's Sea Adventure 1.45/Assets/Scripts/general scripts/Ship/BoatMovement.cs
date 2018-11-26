using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatMovement : MonoBehaviour
{

	[Header("Health")]
	[Space]
	[SerializeField] float health;

	// this is for variables
	[Header("Variables")]
	[Space]

	// for movement
	[SerializeField] float moveSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] float maxReverseSpeed;
	[SerializeField] float deccelerationSpeed;
	[SerializeField] float currentSteerSpeed;
	[SerializeField] float rotationSpeed;
	[SerializeField] bool isInput;

	// this is for boost
	[SerializeField] float boost;
	[SerializeField] float boostUsageRate; // this is the rate of decrease of boost
	[SerializeField] bool isBoosting;

	// to rotate the boat to give it a realistic effect
	[SerializeField] float floatRotation;
	[SerializeField] float rotateRate;
	[SerializeField] bool rotateBack;

	// float on water
	public float heightAboveWater;

	//this is used so that the boat only moves when the player is at the steering wheel 
	public bool controllingBoat;
	Vector3 moveMentOutput;

	// This is for getting components
	[Header("Components")]
	[Space]
	[SerializeField] CharacterController chController;
	[SerializeField] BoatMovement theBoat;

	[Header("UI")]
	[Space]
	public Slider boostSlider;

	[Header("GameObjects")]
	[Space]
	public GameObject theWater;

	// Use this for initialization
	void Start()
	{

		// variables
		
		// for movement
		maxSpeed = 8f;
		moveSpeed = 0f;
		maxReverseSpeed = -5f;
		deccelerationSpeed = 2f;
		currentSteerSpeed = 0f;
		rotationSpeed = 180f;
		isInput = false;
		controllingBoat = false;
		boost = 100f;
		boostUsageRate = 20f;
		isBoosting = false;

		// for floating
		floatRotation = -90f;
		rotateRate = 5f;

		// components
		chController = GetComponent<CharacterController>();
		theBoat = FindObjectOfType<BoatMovement>();

		// For ui
		boostSlider.maxValue = boost;

	}

	// Update is called once per frame
	void Update()
	{

		if (controllingBoat) Movement();
		if (!isInput) StopBoat();

		Boost();

		BoostSlider();

		BoatRocking();

	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// This part is for movement only
	/// 
	// method that handles the boat movement
	void Movement()
	{

		if (moveSpeed > maxSpeed) moveSpeed = (Mathf.FloorToInt(moveSpeed));

		//Local Variables 

		float steerRate = 6f; // this is for the steering later


		float accelRate = 1.5f;

		if (Input.GetKey(KeyCode.W))
		{
			if (moveSpeed < maxSpeed) moveSpeed += accelRate * Time.deltaTime;
			isInput = true;
		}

		if (Input.GetKeyUp(KeyCode.W)) isInput = false;


		if (Input.GetKey(KeyCode.S))
		{
			if (moveSpeed > maxReverseSpeed) moveSpeed -= accelRate * Time.deltaTime;
			isInput = true;
		}

		else if (Input.GetKeyUp(KeyCode.S)) isInput = false;

		//for decceleration 
		if (!isInput)
		{
			if (moveSpeed >= 0) moveSpeed -= deccelerationSpeed * Time.deltaTime;
			if (moveSpeed <= 0) moveSpeed += deccelerationSpeed * Time.deltaTime;
		}

		// Handle the boat rotation.

		if (Input.GetKey(KeyCode.D))
		{

			if (moveSpeed > .5f || moveSpeed < -0.5) currentSteerSpeed += steerRate * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.A))
		{

			if (moveSpeed > .5f || moveSpeed < -0.5) currentSteerSpeed -= steerRate * Time.deltaTime;
		}

		if (moveSpeed < 0.1 && moveSpeed > -0.1f && !isInput) moveSpeed = 0f;

		//to move the character
		moveMentOutput = transform.TransformDirection(Vector3.down * moveSpeed);

		chController.Move(moveMentOutput * Time.deltaTime);

		// to rotate the boat
		transform.rotation = Quaternion.Euler(-90, currentSteerSpeed, 0);
	}

	void Boost()
	{
		if (boost < 0) boost = Mathf.CeilToInt(boost);

		isBoosting = (boost > 0f && Input.GetKey(KeyCode.Space) && theBoat.controllingBoat) ? isBoosting = true : isBoosting = false;
		maxSpeed = (isBoosting) ? 15f : 8f;
		moveSpeed = (isBoosting) ? maxSpeed : moveSpeed;
		if (!isBoosting && moveSpeed > maxSpeed) moveSpeed = 8f;

		boost = isBoosting ? boost -= boostUsageRate * Time.deltaTime : boost;

		if (boost > 100f) boost = 100f;
	}

	void StopBoat()
	{
		moveMentOutput = transform.TransformDirection(Vector3.down * moveSpeed);
		if (moveSpeed >= 0) moveSpeed -= deccelerationSpeed * Time.deltaTime;
		if (moveSpeed <= 0) moveSpeed += deccelerationSpeed * Time.deltaTime;
	}

	//methods to handle the boat animations

	// i did not use animation because it woul affect the steering, cannot steer using animation 

	void BoatRocking()
	{
		if (rotateBack)
		{
			rotateRate -= 2 * Time.deltaTime;
			floatRotation -= 3f * Time.deltaTime;
		}
		else
		{
			rotateRate -= 2 * Time.deltaTime;
			floatRotation += 3f * Time.deltaTime;
		}

		if (rotateRate <= 0 && rotateBack)
		{
			rotateRate = 5f;
			rotateBack = false;
		}

		else if (rotateRate <= 0 && !rotateBack)
		{
			rotateRate = 5f;
			rotateBack = true;
		}

		transform.localRotation = Quaternion.Euler(floatRotation, currentSteerSpeed, transform.rotation.z);

	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// this next part is for UI
	void BoostSlider()
	{
		if (controllingBoat)
		{
			boostSlider.gameObject.SetActive(true);
		}

		else
		{
			boostSlider.gameObject.SetActive(false);
		}

		boostSlider.value = boost;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boost")
		{
			boost += 100;
			Destroy(other.gameObject);
		}
	}

}
