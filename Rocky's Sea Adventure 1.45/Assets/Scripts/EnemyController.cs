using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{
	public enum spawnType { Global, Local }
	public spawnType spawnTypes;

	[Header("EnemyTypes")]
	[SerializeField] int spAtk;
	public enum EnemyType { Normal, Sticky, Laser }
	public EnemyType enemyType;

	private GameObject closestShipSection;
    private Camera cam;

	[Header("coomponents")]
	[SerializeField] Rigidbody rb;

	[Header("Health & UI")]
	[Space]
	[SerializeField] float currentHealth;
	[SerializeField] float maxHealth;
	public Image healthBar;

	[Header("Projectiles and Targetting")]
	public GameObject normalBullet;
    public GameObject stickBullet;
	public GameObject laserBeam;

    [SerializeField] float fireRate;
	[SerializeField] float coolDownTime;    //This value is the attack speed.
	[SerializeField] float moveSpeed, distanceToStopMoving;

	[Header("Variable")]
	public float offsetAboveWater;

    [Header("Drops")]
    [SerializeField] GameObject seaEssence;
    [SerializeField] GameObject woodPlank;
    [SerializeField] GameObject metalPart;
    
    [Header("Detect the ship before it moves")]
    public bool chaseShip;

    [Header("Rise above water before heading for ship")]
    public GameObject sea;

    //LASER ENEMY VARIABLES
    private Vector3 laserDirection;
	private float laserTiming;
	private bool aiming;	//True when showing where to shoot laser, false when laser is actually shot.
	private bool canMove;



	private bool inRange;
    

	public BoatCombat1 ship;
	private DetectShipTrigger detectShipTrigger;
    private bool oilSlicked = false;
    private bool slowedDown = false;


	// Use this for initialization
	public void Start()
	{
		//getting components
		rb = GetComponent<Rigidbody>();


		maxHealth = 100;
		currentHealth = maxHealth;

		fireRate = 0f;
		coolDownTime = 3f;

		moveSpeed = 3.0f;
		canMove = true;
        //distanceToStopMoving = 150.0f;

        cam = Camera.main;
        
        ship = FindObjectOfType<BoatCombat1>();

		detectShipTrigger = GetComponentInChildren<DetectShipTrigger>();

		InvokeRepeating("FindShipTarget", 0, 1f);

		sea = GameObject.Find("Sea");
	}

	// Update is called once per frame
	public void Update()
	{
		if (spawnTypes == spawnType.Global)
            chaseShip = true;

		if (chaseShip && transform.position.y > sea.transform.position.y + offsetAboveWater || spawnTypes == spawnType.Global) MoveToShip();

		EnemyTypes();

        DeactivateHealthBar();

		MoveAboveWater();
	}

	void MoveToShip()
	{
		if (canMove)
		{
			Vector3 direction = ship.transform.position - transform.position;
			//if (distanceToStopMoving < (ship.transform.position - transform.position).sqrMagnitude)
			if ((ship.transform.position - transform.position).sqrMagnitude < distanceToStopMoving)
			{
				rb.velocity = direction.normalized * -moveSpeed / 1.3f;
			}
			else if (distanceToStopMoving < (ship.transform.position - transform.position).sqrMagnitude)
				rb.velocity = direction.normalized * moveSpeed;

			//transform.position = Vector3.MoveTowards(transform.position, closestShipSection.transform.position, moveSpeed * Time.deltaTime);

			Vector3 targetDir = ship.transform.position - transform.position;

			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir,Mathf.Infinity,Mathf.Infinity);
			transform.rotation = Quaternion.LookRotation(targetDir);
		}
		else
			rb.velocity = Vector3.zero;
    }



    //An InvokeRepeating initialized at start to find which section of ship to move to and shoot at.
    //Repeats every second
    void FindShipTarget()
	{
		var distance = Mathf.Infinity;

		foreach (GameObject section in ship.cannonHolder)
		{
			var difference = (section.transform.position - transform.position);
			var curDistance = difference.sqrMagnitude;
			if (curDistance < distance)
			{
				distance = curDistance;

				closestShipSection = section;
			}
		}
	}

	void FireRate()
	{
		if (fireRate > 0f)
		{
			fireRate -= Time.deltaTime;
		}
	}

	void Shoot()
	{
		if (fireRate <= 0)
		{
			fireRate = coolDownTime;
			GameObject enemyBullet = Instantiate(normalBullet, transform.position, Quaternion.identity);
			enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (closestShipSection.transform.position - transform.position).normalized;
		}
	}

	void EnemyTypes()
	{
		if (detectShipTrigger.shipDetected && enemyType == EnemyType.Normal)
		{
			FireRate();
			Shoot();
		}

		if (detectShipTrigger.shipDetected && enemyType == EnemyType.Sticky)
		{
			FireRate();
			StickyEnemy();
		}

		if (detectShipTrigger.shipDetected && enemyType == EnemyType.Laser || aiming)
		{
			FireRate();
			LaserEnemy();
		}
	}

	void StickyEnemy()
	{
		if (enemyType == EnemyType.Sticky)
		{
			if (fireRate <= 0 && spAtk <= 0)
			{
				GameObject enemyBullet = Instantiate(stickBullet.gameObject, transform.position, Quaternion.identity);
				enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (closestShipSection.transform.position - transform.position).normalized;
				spAtk = Random.Range(3, 7);
				fireRate = coolDownTime;
			
			}

			else if (fireRate <= 0 && spAtk > 0)
			{
				Shoot();
				spAtk--;
			}
		}
	}

	void LaserEnemy()
	{
		//Needed to continue laser stuff even when ship moves out of range.
		if (detectShipTrigger.shipDetected == false)
			detectShipTrigger.shipDetected = true;

		//Start the beam. Show where it shoots.
		if (fireRate <= 0 && !aiming)
		{
			aiming = true;
			canMove = false;


			GameObject laserAim = gameObject.transform.Find("Laser Indicator").gameObject;
			laserAim.SetActive(true);
			laserAim.transform.LookAt(closestShipSection.transform.position);

            laserDirection = closestShipSection.transform.position;

            laserTiming = 8;
			fireRate = Mathf.Infinity;
		}
		laserTiming -= Time.deltaTime;
		FireLaserBeam();
	}

	//Actual firing of the beam
	void FireLaserBeam()
	{
		//TIme to shoot the beam
		if(laserTiming <= 0 && aiming)
		{
			//Turns off Aiming beam
			gameObject.transform.Find("Laser Indicator").gameObject.SetActive(false);

			//GameObject enemyBullet = Instantiate(normalBullet, transform.position, Quaternion.identity);
			//enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (closestShipSection.transform.position - transform.position).normalized;

			GameObject laser = Instantiate(laserBeam, transform.position, Quaternion.identity);

			laser.GetComponent<EnemyBulletScript>().moveDirection = (laserDirection - transform.position).normalized;
			fireRate = coolDownTime;

			canMove = true;

            aiming = false;
            detectShipTrigger.shipDetected = false;
        }
	}

	//------------------------------------------
	//health related stuff

	public void Health(float damageTaken)
	{
		currentHealth -= damageTaken;
		if (currentHealth <= 0f)
		{
			Destroy(gameObject);
        }
        HealthUi();
    }

	void HealthUi()
	{
		healthBar.fillAmount = currentHealth / maxHealth;
	}

    void DeactivateHealthBar()
    {
        //Performs checks to disable health UI only if player not controlling boat
        if (!ship.GetComponent<BoatController>().controllingBoat)
        {
            //Get magnitude of enemy to camera
            Vector3 enemyToShip = (ship.transform.position - transform.position);
            Vector3 enemyToCamera = (cam.transform.position - transform.position);
            if (Vector3.Dot(enemyToShip, enemyToCamera) < 0)
            {
                healthBar.transform.parent.gameObject.SetActive(false);
            }
            else
                healthBar.transform.parent.gameObject.SetActive(true);
        }
        else
            healthBar.transform.parent.gameObject.SetActive(true);
    }

	void MoveAboveWater()
	{
		if (transform.position.y < sea.transform.position.y + offsetAboveWater)
		{
			transform.Translate(0, 1 * Time.deltaTime, 0, Space.World);
		}
	}


	//
    //---------------------------------------------
    private void OnTriggerStay(Collider other)
    {
        // If Enemy is in oil slick and is not affected by oil slick,
        // Start damaging Enemy over time & give slow effect.
        if (other.tag == "Oil Slick" && !oilSlicked)
        {
            StartCoroutine(DotDamage(1, 4));

            if (!slowedDown)
            {
                SlowDownEnemy(0.5f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If Enemy leaves oil slick, no longer affected by oil slick.
        if (other.tag == "Oil Slick")
        {
            oilSlicked = false;
            slowedDown = false;
            SlowDownEnemy(-1); // Returns enemy's speed back to normal, unreduced state.
        }

    }


    // ----- Oil slick's Damage over Time (DoT), call this when Enemy is affected by oil slick.
    IEnumerator DotDamage(float dmgDelay, float dmgAmount)
    {
        oilSlicked = true;

        while (oilSlicked)
        {
            Health(dmgAmount);
            yield return new WaitForSeconds(dmgDelay);
        }

        oilSlicked = false;
    }

    // ----- Slows down enemy's moveSpeed, call this when Enemy is affected by oil slick.
    // --- Float slowFactor e.g. Slow Factor of 0.1 = reduce Enemy moveSpeed by 10%, DO NOT MAKE SLOW FACTOR > 1 unless it is meant to increase speed.
    public void SlowDownEnemy(float slowFactor)
    {

        if (!slowedDown)
        {
            moveSpeed = moveSpeed * (1 - slowFactor);
            slowedDown = true;
        }
    }

    private void OnDestroy()
    {
        float randomValue = Random.value;
        GameObject resource = null;

        if (randomValue < 0.133f)
            resource = Instantiate(seaEssence, transform.position, Quaternion.identity);
        else if(randomValue < 0.266f)
            resource = Instantiate(woodPlank, transform.position, Quaternion.identity);
        else if (randomValue < 0.399f)
            resource = Instantiate(metalPart, transform.position, Quaternion.identity);

        resource.transform.localScale = new Vector3(1, 1, 1);
        print(randomValue);
    }
}