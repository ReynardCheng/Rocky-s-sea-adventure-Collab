using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	[Header("Variables")]
	[Space]
	[SerializeField] float moveSpeed;
	[SerializeField] bool canMove;
	public bool canControlShip;
	//for reference
	[Header("Components")]
	[Space]
	CharacterController controller;
	BoatMovement theBoat;

	// have to use this for reference because it is from a namespace
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

	// Use this for initialization
	void Start () {
		canControlShip = false;
		moveSpeed = 3f;
		controller = GetComponent<CharacterController>();
		theBoat = FindObjectOfType<BoatMovement>();
		fpsController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {

		ControllingTheBoat();

	}


	// For methods
	void ControllingTheBoat()
	{

		// to ensure that the player moves with the boat
		//transform.parent = (theBoat.controllingBoat) ? theBoat.transform : transform.parent = null;
		transform.parent = theBoat.transform;

		if (canControlShip)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				fpsController.controllingShip = true;
			}
		}
		else if (canControlShip && fpsController.controllingShip)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				fpsController.controllingShip = false;
			}
		}
	}


	// we dont really need this since we have the fps controller
	void MoveMent()
	{
		Vector2 moveDir = GameInputManager.GetP1MoveMent();
		Vector3 movement = transform.TransformDirection(new Vector3(moveDir.x, 0, moveDir.y));


		// for the player to move
		controller.Move(movement * moveSpeed*Time.deltaTime);
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "SteeringWheel")
		{
			canControlShip = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "SteeringWheel")
		{
			canControlShip = false;
		}
	}

}
