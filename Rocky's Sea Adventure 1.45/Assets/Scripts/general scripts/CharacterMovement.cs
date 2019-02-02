using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] Rigidbody rb;

	public cannonTypes menutype;

	[Header("MiniMap Opened")]
	public bool mapOpened;
	public bool crRunning;
	float lerpRate;
	public Image blackScreen;
	public Camera raycastCam;
	public GameObject miniMap;
	public GameObject[] shipUI;

	// Use this for initialization
	void Start () {
	
		raycastCam.enabled = false;
		canControlShip = false;
		moveSpeed = 3f;
		//controller = GetComponent<CharacterController>();
		theBoat = FindObjectOfType<BoatController>();
	//	fpsController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

		ControllingTheBoat();

		ChangeMap();

	}


	// For methods
	void ControllingTheBoat()
	{

		// to ensure that the player moves with the boat
		//transform.parent = (theBoat.controllingBoat) ? theBoat.transform : transform.parent = null;
		transform.parent = theBoat.transform;
	}


	void ChangeMap()
	{
		lerpRate += Time.deltaTime / 5;
		if (!mapOpened)
		{
			if (Input.GetKeyDown(KeyCode.M) && !crRunning)
			{
				mapOpened = true;
				StartCoroutine(AnimateMinimap());
			
			}
		}

		if (mapOpened)
		{
			{
				if (Input.GetKeyDown(KeyCode.M) && !crRunning)
				{
					mapOpened = false;
					StartCoroutine(AnimateMinimap());
				}
			}
		}
	}

	IEnumerator AnimateMinimap()
	{
		blackScreen.gameObject.SetActive(true);
		crRunning = true;
		lerpRate = 0;
		Color designatedColor = Color.black;

		while (blackScreen.color != designatedColor)
		{
			blackScreen.color = Color.Lerp(blackScreen.color, designatedColor, lerpRate);
			yield return null;
		}

		if (mapOpened)
		{
			mainCam.enabled = false;
			raycastCam.enabled = true;
			miniMap.SetActive(false);
			foreach (GameObject g in shipUI)
			{
				g.SetActive(false);
			}
		}
		else if (!mapOpened)
		{
			mainCam.enabled = true;
			raycastCam.enabled = false;
			miniMap.SetActive(true);
			foreach (GameObject g in shipUI)
			{
				g.SetActive(true);
			}
		}

		if(designatedColor == Color.black)
		designatedColor = Color.clear;

		while (blackScreen.color != designatedColor)
		{
			blackScreen.color = Color.Lerp(blackScreen.color, designatedColor, lerpRate);
			yield return null;
		}

		blackScreen.gameObject.SetActive(false);
		crRunning = false;
	}

	// we dont really need this since we have the fps controller
	void MoveMent()
	{
		Vector2 moveDir = GameInputManager.GetP1MoveMent();
		Vector3 movement = transform.TransformDirection(new Vector3(moveDir.x, 0, moveDir.y));


		// for the player to move
		//controller.Move(movement * moveSpeed*Time.deltaTime);
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
