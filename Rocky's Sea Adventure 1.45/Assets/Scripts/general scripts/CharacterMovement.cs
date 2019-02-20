using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {

	[Header("Variables")]
	[Space]
	[SerializeField] float moveSpeed;
	[SerializeField] public bool canMove;
	public bool canControlShip;
	public bool gameStart;
	public static Transform characterPos;

	//for reference
	[Header("Components")]
	[Space]
	//CharacterController controller;
	BoatController theBoat;
    public GameObject steeringMenu;
	[SerializeField] Rigidbody rb;

	[Header("Camera")]
	public Camera mainCam;
	public Transform rotateTarget;

	public cannonTypes menutype;

	[Header("MiniMap Opened")]
	public bool mapOpened;
	public bool crRunning;
	float lerpRate;
	public Image blackScreen;
	public Camera raycastCam;
	public GameObject miniMap;
	public GameObject[] shipUI;

    [Header("PlayerAnimation")]
    public Transform steeringWheelStandPosition;
    public GameObject steeringWheelBoat, steeringWheelPlayer;
    [SerializeField] private Animator meshAnim;
    [SerializeField] CameraSwitch theCamera;
    public bool steering, steeringLeft, steeringRight;
    private bool walking;

	// Use this for initialization
	void Start () {
        meshAnim = GetComponentInChildren<Animator>();
        theCamera = FindObjectOfType<CameraSwitch>();
		gameStart = false;
		raycastCam.enabled = false;
		canControlShip = false;
		moveSpeed = 3f;
		theBoat = FindObjectOfType<BoatController>();
		rb = GetComponent<Rigidbody>();
		rotateTarget = new GameObject("Rotate Target").transform;
        mainCam = Camera.main;

        if (!FindObjectOfType<CutSceneController>())
        {
            gameStart = true;
            canMove = true;
        }

		characterPos = this.transform; // sets this transform such that other scripts can reference this

	}

	// Update is called once per frame
	void Update()
	{
        meshAnim.SetBool("Walking", walking);
        meshAnim.SetBool("Steering", steering);
        meshAnim.SetBool("SteeringLeft", steeringLeft);
        meshAnim.SetBool("SteeringRight", steeringRight);
        if (!theBoat.controllingBoat)
        {
            steeringLeft = false;
            steeringRight = false;
        }

        PlayerAnimation();

		if (gameStart && canMove)
		{
			float horizontal = Input.GetAxis("Horizontal(P1)");
			float vertical = Input.GetAxis("Vertical(P1)");

			ControllingTheBoat();
            if (!theBoat.controllingBoat)
            {
                if (horizontal != 0 || vertical != 0) characterRotateMovement();

                steering = false;
                steeringWheelBoat.SetActive(true);
                steeringWheelPlayer.SetActive(false);
            }
            if (theBoat.controllingBoat)
            {
                steering = true;
                transform.localRotation = Quaternion.Euler(90, 0, 0);
                transform.position = steeringWheelStandPosition.position;

                steeringWheelBoat.SetActive(false);
                steeringWheelPlayer.SetActive(true);
            }

			ChangeMap();
		}

		if (!canMove)
		{
			gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
			//theBoat.movementFactor = Mathf.Lerp(theBoat.movementFactor, 0, Time.deltaTime * 3);
			//theBoat.steerFactor = Mathf.Lerp(theBoat.steerFactor, 0, Time.deltaTime * 3);
			theBoat.movementFactor = 0f;
			theBoat.steerFactor = 0f;
		}
		else {
			gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
		}
     
	}

    void PlayerAnimation()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (!theBoat.controllingBoat && !theCamera.switching) walking = true;
        }
        else
        {
            walking = false;
        }
    
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

	void characterRotateMovement()
	{
			//Vector3 desiredTransform = new Vector3(-mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
			Vector3 desiredTransform = transform.position + transform.position - mainCam.transform.position;
			desiredTransform = new Vector3(desiredTransform.x, transform.position.y, desiredTransform.z);
			rotateTarget.transform.position = desiredTransform;
			transform.LookAt(rotateTarget);
	}

    private void OpenSteerMenu()
    {
        steeringMenu.SetActive(true);
    }

    private void CloseSteerMenu()
    {
        steeringMenu.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
	{
		if (other.tag == "SteeringWheel")
		{
			canControlShip = true;
         if(TutorialInstructions.count <= 0)   TutorialInstructions.count = 1;
            OpenSteerMenu();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "SteeringWheel")
		{
			canControlShip = false;

            CloseSteerMenu();
		}
	}

}
