using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShooter : MonoBehaviour {

    public bool shipDetetected;
    public DetectShipTrigger detectShipTrigger;

    BoatCombat ship;
    float distanceToLeft;
    float distanceToRight;
    GameObject closestShipSide;
    [SerializeField] float moveSpeed, distanceToStopMoving;
    public LayerMask shipSideMask;  //Create a new layer for the two Ship side empty gameobjects.

    public GameObject bullet;
    [SerializeField] float fireRate;
    [SerializeField] float coolDownTime;

	[Header("Health & UI")]
	[Space]
	[SerializeField] float currentHealth;
	[SerializeField] float maxHealth;
	public Image healthBar; 

	// Use this for initialization
	void Start () {
		fireRate = 0f;
		coolDownTime = 3f;

        moveSpeed = 3.0f;
        distanceToStopMoving = 150.0f;

        detectShipTrigger = GetComponentInChildren<DetectShipTrigger>();
        ship = FindObjectOfType<BoatCombat>();

		//--------------------
		//Health related
		maxHealth = 100;
		currentHealth = maxHealth;

	}
	
	// Update is called once per frame
	void Update () {
        if (detectShipTrigger.shipDetected) //changed
        {
            FireRate();
            Shoot();
            shipDetetected = true;
        }
        MoveToShipClosestPart();

		HealthUi();
    }


    void Shoot()
    {
        if (fireRate <= 0)
        {
            fireRate = coolDownTime;

            GameObject weakestCannon = FindLowestHPCannon();
            print(weakestCannon);
            if (weakestCannon != null)
            {
                GameObject enemyBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (weakestCannon.transform.position - transform.position).normalized;
            }
            else
            {
                GameObject enemyBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (ship.transform.position - transform.position).normalized;
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

	

    void MoveToShipClosestPart()
    {
        distanceToLeft = (gameObject.transform.position - ship.shipLeftSide.transform.position).sqrMagnitude;
        distanceToRight = (gameObject.transform.position - ship.shipRightSide.transform.position).sqrMagnitude;

        if (distanceToLeft < distanceToRight)
        {
            closestShipSide = ship.shipLeftSide;
        }
        if (distanceToLeft > distanceToRight)
        {
            closestShipSide = ship.shipRightSide;
        }
        if (distanceToStopMoving < (closestShipSide.transform.position - transform.position).sqrMagnitude)
        {
            transform.position = Vector3.MoveTowards(transform.position, closestShipSide.transform.position, moveSpeed * Time.deltaTime);
        } 

    }

    GameObject FindLowestHPCannon()
    {
        GameObject weakestCannon = null;

        float lowestHealth = Mathf.Infinity;
        if (closestShipSide == ship.shipRightSide)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ship.cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    float cannonHealth = ship.cannonHolder[i].GetComponent<CannonHealth>().health;
                    if (cannonHealth < lowestHealth)
                    {
                        weakestCannon = ship.cannonHolder[i];
                        lowestHealth = cannonHealth;
                    }
                }
            }
        }
        else if (closestShipSide == ship.shipLeftSide)
        {
            for (int i = 3; i < 6; i++)
            {
                if (ship.cannonHolder[i].GetComponent<BuildCannon>().slotTaken)
                {
                    {
                        float cannonHealth = ship.cannonHolder[i].GetComponent<CannonHealth>().health;
                        if (cannonHealth < lowestHealth)
                        {
                            weakestCannon = ship.cannonHolder[i];
                            lowestHealth = cannonHealth;
                        }
                    }
                }
            }
        }

        return weakestCannon;
    }

	//------------------------------------------
	//health related stuff

	void Health(float damageTaken)
	{
		currentHealth -= damageTaken;
		if (currentHealth <= 0f)
		{
			Destroy(gameObject);
		}
	}

	void HealthUi()
	{
		healthBar.fillAmount = currentHealth / maxHealth;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cannon Bullet")
		{
			Health(10f);
			print("Enemy Hit");
		}
	}
}


/*
public class EnemyPathfinding : MonoBehaviour
{

    [Header("Ship Sides")]
    public GameObject shipLeftSide;
    public GameObject shipRightSide;

    public GameObject closestShipSide;

    private float distanceToLeft;
    private float distanceToRight;

    private EnemyBulletScript enemyBullet;
    private EnemyShooter enemyShooterScript;

    public GameObject[] CannonsInTargetSide;
    public Transform TargetingCannon;


    private BoatCombat boatCombatScript;

    // Use this for initialization
    void Start()
    {

        boatCombatScript = FindObjectOfType<BoatCombat>();

        CannonsInTargetSide = boatCombatScript.cannonHolder;


        enemyBullet = GetComponent<EnemyBulletScript>();
        enemyShooterScript = GetComponent<EnemyShooter>();

    }

    // Update is called once per frame
    void Update()
    {
        distanceToLeft = (gameObject.transform.position - shipLeftSide.transform.position).sqrMagnitude;
        distanceToRight = (gameObject.transform.position - shipRightSide.transform.position).sqrMagnitude;

        FindClosestShipPart();
        //FindTargetCannon();

    }

    void FindClosestShipPart()
    {
        if (distanceToLeft < distanceToRight)
        {
            closestShipSide = shipLeftSide;
        }

        if (distanceToLeft > distanceToRight)
        {
            closestShipSide = shipRightSide;
        }
    }

    //void FindTargetCannon()
    //{

    //    int lowestHealth = 100;

    //    foreach (GameObject cannon in CannonsInTargetSide)
    //    {
    //        CannonsInTargetSide.Add(cannon);
    //        print ("thingthing");
    //        if (cannon.GetComponent<TestCannonHP>().cannonHP < lowestHealth)
    //        {
    //            print("thing");
    //            lowestHealth = cannon.GetComponent<TestCannonHP>().cannonHP;
    //            print("Went thr0u");
    //            TargetingCannon = cannon.transform;
    //            print("Went thru");
    //        }
    //    }
    //}
} */