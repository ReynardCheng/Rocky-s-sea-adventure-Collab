using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof (FloatMovement))]
public class BoatController : MonoBehaviour
{

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
	[SerializeField] float boost;
	[SerializeField] float boostUsageRate; // this is the rate of decrease of boost
	[SerializeField] bool isBoosting;

	// steering
	float horizontalInput;
	public float steerFactor;
	public float steerSpeed;
	
	//balance
	public Vector3 centerOfMass;
	Transform thisCenter;

	//this is used so that the boat only moves when the player is at the steering wheel 
	public bool controllingBoat;

	// This is for getting components
	[Header("Components")]
	[Space]
	[SerializeField] CharacterController chController;
	[SerializeField] Rigidbody rb;

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

    // Use this for initialization
    void Start()
	{
		
        m_AudioSource = GetComponent<AudioSource>();

		// variables
		// for movement
		moveSpeed = 2f;
		movementThreshold = 4f;
		controllingBoat = false;
		boost = 100f;
		boostUsageRate = 20f;
		isBoosting = false;

		// components
		chController = GetComponent<CharacterController>();
		rb = GetComponent<Rigidbody>();

		// For ui
		boostSlider.maxValue = boost;

	}

	// Update is called once per frame
	void Update()
	{
		// transform.rotation = Quaternion.Euler(0, 0, 0);
		if (controllingBoat)
		{
			Movement();
			Steer();
		}
		Balance();

		Boost();

		BoostSlider();

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
	}

	void Movement()
	{
		verticalInput = Input.GetAxis("Vertical(P1)");
		movementFactor = Mathf.Lerp(movementFactor, verticalInput, Time.deltaTime / movementThreshold);

		RaycastHit hit;
		foreach(Transform t in raycastColliders)
		{
			Ray ray = new Ray(t.position, t.forward);
			Physics.Raycast(ray, out hit, 1f, collisionsThatAffectBoat);

			// Hitting something that affects boat
			if (hit.collider != null)
			{
				return;
			}
		}
		
		transform.Translate(0.0f, 0.0f, movementFactor / moveSpeed);
	}

	private void OnDrawGizmos()
	{
		if (controllingBoat)
		{
			foreach (Transform t in raycastColliders)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(t.position, t.position + t.forward * 1f);
			}
		}
	}

	void Steer()
	{
		if (steerFactor >= 0.1f) steerFactor = 0.05f;
		if (steerFactor <= -0.1f) steerFactor = -0.05f;
		print("Steering");
		horizontalInput = Input.GetAxis("Horizontal(P1)");
		steerFactor = Mathf.Lerp(steerFactor, horizontalInput*steerSpeed, Time.deltaTime / movementThreshold);
		transform.Rotate(0.0f, -steerFactor, 0.0f);
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


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boost")
		{
			boost += 100;
			Destroy(other.gameObject);
		}
	}

}
