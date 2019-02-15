using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof (FloatMovement))]
public class BoatController : MonoBehaviour
{

    public bool reachedEnd;

	[Header("Health")]
	[Space]
	[SerializeField] float health;

	// this is for variables
	[Header("Variables")]
	[Space]

	// for movement
	[SerializeField] float moveSpeed;
	// new Movement
	float verticalInput;
	public float movementFactor;
	float movementThreshold;

	// this is for boost
	[SerializeField] public float boost;
	[SerializeField] float boostUsageRate; // this is the rate of decrease of boost
	[SerializeField] bool isBoosting;

	// steering
	float horizontalInput;
	public float steerFactor;
	public float steerSpeed;
	float maxSteerSpeed;

	//balance
	public Vector3 centerOfMass;
	Transform thisCenter;

	//this is used so that the boat only moves when the player is at the steering wheel 
	public bool controllingBoat;

	// for stoopping of movement
	public bool hitWall;

	// This is for getting components
	[Header("Components")]
	[Space]
	//[SerializeField] CharacterController chController;
	[SerializeField] Rigidbody rb;
	[SerializeField] myGUI theGUI;
	[SerializeField] BoatCombat1 theBoatCombat;
    [SerializeField] LevelManager levelManager;

	[Header("UI")]
	[Space]
	public Slider boostSlider;

	[Header("GameObjects")]
	[Space]
	public GameObject theWater;

    [Header("Sound")]
    [SerializeField] AudioClip BoatMoving;
    [SerializeField] AudioClip BoatBoosted;
    private AudioSource m_AudioSource;

	public LayerMask collisionsThatAffectBoat;
	public Transform[] raycastColliders;

	[Header("miniMap")]
	CharacterMovement theCharacter;


    //Ramming
    public int moveFactor;
    public int RamDamageToShip = 5;
    public float RamDamageToEnemy = 10f;
	
	// Use this for initialization
	void Start()
	{
		maxSteerSpeed = steerSpeed;
        m_AudioSource = GetComponent<AudioSource>();

		// variables
		// for movement
		moveSpeed = 5f;
		movementThreshold = 15f;
		controllingBoat = false;
		boost = 100f;
		boostUsageRate = 20f;
		isBoosting = false;

		// components
	//	chController = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();
		theGUI = FindObjectOfType<myGUI>();
		theBoatCombat = GetComponent<BoatCombat1>();
		theCharacter = FindObjectOfType<CharacterMovement>();
        levelManager = FindObjectOfType<LevelManager>();

		// For ui
		boostSlider.maxValue = boost;
	}

	// Update is called once per frame
	void Update()
	{
		if (!LevelManager.theLevelManager.gamePaused)
		{
			// transform.rotation = Quaternion.Euler(0, 0, 0);
			if (controllingBoat && !theCharacter.mapOpened && !theCharacter.crRunning)
			{
				Movement();
				Steer();
			}
			Balance();

			Boost();

			BoostSlider();

			if (!controllingBoat)
			{
				movementFactor = 0;
				steerFactor = 0;
			}

			if (theBoatCombat.shipHealth <= 0) theGUI.lose = true;
		}
    }

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// This part is for movement only
	/// 
	// method that handles the boat movement

	void Balance()
	{
		if (!thisCenter)
		{
			thisCenter = new GameObject("Center").transform;
			thisCenter.SetParent(transform);
		}

		//thisCenter.position = centerOfMass + transform.position;
		//GetComponent<Rigidbody>().centerOfMass = thisCenter.position;
	}

	void Movement()
	{
		verticalInput = Input.GetAxis("Vertical(P1)");
		movementFactor = Mathf.Lerp(movementFactor, verticalInput, Time.deltaTime / movementThreshold);

        moveFactor = (int)(moveSpeed * verticalInput);

        float boostFactor = 1;

		RaycastHit hit;
		foreach(Transform t in raycastColliders)
		{
			Ray ray = new Ray(t.position, t.forward);

			Physics.Raycast(ray, out hit, 5f, collisionsThatAffectBoat);

			// Hitting something that affects boat
			if (hit.collider != null)
			{
                print("hitWall");
				return;
			}
		}

		if (hitWall) return;

		boostFactor = (isBoosting) ? 1.5f : 1;
		transform.Translate(0.0f, 0.0f, (movementFactor * boostFactor * moveSpeed/5));
		
	}

	private void OnDrawGizmos()
	{
		if (controllingBoat)
		{
			foreach (Transform t in raycastColliders)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(t.position, t.position + t.forward * 1f);
				//print("isdrawing");
			}
		}
	}

	void Steer()
	{
		//if (steerFactor >= 0.3f) steerFactor = 0.3f;
		//if (steerFactor <= -0.3f) steerFactor = -0.3f;

		horizontalInput = Input.GetAxis("Horizontal(P1)");
		//if (horizontalInput > 0 || horizontalInput < 0) steerSpeed = Mathf.Lerp(0, maxSteerSpeed, Time.deltaTime * 20);
		//steerFactor = steerSpeed * horizontalInput;
		//steerFactor = Mathf.Lerp(steerFactor, horizontalInput * steerSpeed, Time.deltaTime / movementThreshold);
		steerFactor = Mathf.Lerp(steerFactor, horizontalInput * steerSpeed, Time.deltaTime * 6);
		//steerFactor = (horizontalInput * steerSpeed * Time.deltaTime / 10);
		transform.Rotate(0.0f, steerFactor, 0);
	//	print(horizontalInput);
	}

	void Boost()
	{
		if (boost < 0) boost = Mathf.CeilToInt(boost);

		isBoosting = (boost > 0f && Input.GetKey(KeyCode.Space) && controllingBoat) ? isBoosting = true : isBoosting = false;
		boost = isBoosting ? boost -= boostUsageRate * Time.deltaTime : boost;

		if (boost > 100f) boost = 100f;
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


	//-------------------------
	// for when the game ends

        //when collect boost
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boost")
		{
			boost += 100;
			Destroy(other.gameObject);
		}

        if (other.tag == "End Point")
        {
            levelManager.Win();
        }

        /// *************************************************************
        /// Ship ramming tiem
        /// *************************************************************

        ///move factor as dmg multiplier
        // calculation: moveFactor = (int)(moveSpeed * verticalInput) ====> max moveFactor = 25

        if (other.tag == "Enemy")
         {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            RamDamageToEnemy = 2;
            enemyController.Health(RamDamageToEnemy*moveFactor);
            //max damage to enemy = 2 x 25 = 50
            print("ramming damage to enemy:"+ RamDamageToEnemy*moveFactor);

            //damage ship when ram enemy
            BoatCombat1 theBoatCombat = GetComponentInParent<BoatCombat1>();
            RamDamageToShip = 1; // max damage to ship = 1 x (5x5) /5 = 5
            theBoatCombat.DamageShip(RamDamageToShip * moveFactor/5);

         }

        if (other.gameObject.layer == 10)
        {
            //damage ship when ram rocks
            BoatCombat1 theBoatCombat = GetComponentInParent<BoatCombat1>();
            RamDamageToShip = 3; //max damage to ship = 3 x (5x5) / 5 = 15 
            theBoatCombat.DamageShip(RamDamageToShip * moveFactor/5);
        }

    }
	
}
