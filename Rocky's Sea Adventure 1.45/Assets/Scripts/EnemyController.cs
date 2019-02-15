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

	[SerializeField] private GameObject closestShipSection;
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

    public float offsetAboveWater;

    [Header("Local Enemy Variables")]
    private Vector3 localInitialPosition;
    private bool isHome;
    public bool returningHome;
    [SerializeField] private float patrolRange;
    private bool currentlyPatrolling;
    private Vector3 patrolDestination;
    private bool isAtPatrolDestination;
    private float timeBeforeNextPatrol;

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

	CharacterMovement player;


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
        timeBeforeNextPatrol = 5;

        cam = Camera.main;
        
        ship = FindObjectOfType<BoatCombat1>();

		detectShipTrigger = GetComponentInChildren<DetectShipTrigger>();

		InvokeRepeating("FindShipTarget", 0, 1f);

		sea = GameObject.Find("Sea");

		player = FindObjectOfType<CharacterMovement>();

        if (spawnTypes == spawnType.Local)
        {
            localInitialPosition = transform.position;
            patrolDestination = localInitialPosition;
            isHome = true;
            isAtPatrolDestination = true;
        }
	}

	// Update is called once per frame
	public void Update()
	{
		if (player.gameStart)
		{
			if (spawnTypes == spawnType.Global)
				chaseShip = true;

            if (chaseShip && transform.position.y > sea.transform.position.y + offsetAboveWater || spawnTypes == spawnType.Global)
            {
                isHome = false;
                MoveToShip();
            }

			EnemyTypes();

			DeactivateHealthBar();

			MoveAboveWater();

            if (spawnTypes == spawnType.Local)
            {
                if (!chaseShip && returningHome)
                {
                    MoveBackHome();
                }

                if(!chaseShip && isHome)
                {
                    Patrol();
                }

            }
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

			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Mathf.Infinity, Mathf.Infinity);
			transform.rotation = Quaternion.LookRotation(targetDir);
		}
		else
			rb.velocity = Vector3.zero;
    }

    void MoveBackHome()
    {
        Vector3 direction = localInitialPosition - transform.position;

        if (transform.position == localInitialPosition)
        {
            isHome = true;
            returningHome = false;
        }
        if (transform.position != localInitialPosition)
        {
            rb.velocity = direction.normalized * moveSpeed;
            isHome = false;
        }
    }

    void Patrol()
    {
        if (!currentlyPatrolling)
        {
            if (isAtPatrolDestination == true)
            {
                timeBeforeNextPatrol -= Time.deltaTime;
            }

            if (timeBeforeNextPatrol < 0)
            {
                //Get a new destionation to patrol to
                patrolDestination = localInitialPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
                Vector3 direction = patrolDestination - transform.position;
                //Set velocity to move towards this destionation
                rb.velocity = direction.normalized * moveSpeed;
                currentlyPatrolling = true;
                isAtPatrolDestination = false;
            }
        }

        if (currentlyPatrolling)
        {
            if (Vector3.Distance(transform.position, patrolDestination) < 3)
            {
                rb.velocity = Vector3.zero;
                timeBeforeNextPatrol = Random.Range(3f, 5f);
                currentlyPatrolling = false;
                isAtPatrolDestination = true;
            }
        }

    }

    void ResetPatrol()
    {
        //Get a new destionation to patrol to
        patrolDestination = localInitialPosition + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
        Vector3 direction = patrolDestination - transform.position;
        //Set velocity to move towards this destionation
        rb.velocity = direction.normalized * moveSpeed;
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
			//enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (closestShipSection.transform.position - transform.position).normalized;

			Vector3 whereToTarget = new Vector3(closestShipSection.transform.position.x, closestShipSection.transform.position.y - 2, closestShipSection.transform.position.z);
			
			enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (whereToTarget - transform.position).normalized;
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
		//Start the beam. Show where it shoots
		if (fireRate <= 0 && !aiming)
		{
			aiming = true;
			canMove = false;

            Vector3 aimDirection = new Vector3(closestShipSection.transform.position.x, closestShipSection.transform.position.y - 1, closestShipSection.transform.position.z);
            
            //Laser Indicator is the red cylinder that shows where it shoots.
            GameObject laserAim = gameObject.transform.Find("Laser Indicator").gameObject;
			laserAim.SetActive(true); 

            //Points indicator towards where it shoots
			laserAim.transform.LookAt(aimDirection);

            //LaserDirection is where the projectile will shoot at.
            laserDirection = aimDirection;
            

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
		{
			resource = Instantiate(seaEssence, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
			resource.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (randomValue < 0.266f)
		{
			resource = Instantiate(woodPlank, transform.position, Quaternion.identity);
			resource.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (randomValue < 0.399f)
		{
			resource = Instantiate(metalPart, transform.position, Quaternion.identity);
			resource.transform.localScale = new Vector3(1, 1, 1);
		}
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            ResetPatrol();
        }
    }
}