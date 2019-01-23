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
	//CharacterController controller;
	BoatController theBoat;
	[SerializeField]Rigidbody rb;

	[Header("Camera")]
	public Camera mainCam;
	public Transform rotateTarget;

	public cannonTypes menutype;
	// Use this for initialization
	void Start () {
		canControlShip = false;
		moveSpeed = 3f;
		theBoat = FindObjectOfType<BoatController>();
		rb = GetComponent<Rigidbody>();
		rotateTarget = new GameObject("Rotate Target").transform;
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal(P1)");
		float vertical = Input.GetAxis("Vertical(P1)");

		ControllingTheBoat();
		if (horizontal != 0 || vertical != 0) characterRotateMovement();

	}


	// For methods
	void ControllingTheBoat()
	{

		// to ensure that the player moves with the boat
		//transform.parent = (theBoat.controllingBoat) ? theBoat.transform : transform.parent = null;
		transform.parent = theBoat.transform;
	}

	

	void characterRotateMovement()
	{

		//Vector3 desiredTransform = new Vector3(-mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		Vector3 desiredTransform = transform.position + transform.position - mainCam.transform.position;
		desiredTransform = new Vector3(desiredTransform.x, transform.position.y, desiredTransform.z);
		rotateTarget.transform.position = desiredTransform;
		transform.LookAt(rotateTarget);
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
