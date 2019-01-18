using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

	private GameObject closestShipSection;

	[Header("Health & UI")]
	[Space]
	[SerializeField] float currentHealth;
	[SerializeField] float maxHealth;
	public Image healthBar;

	[Header("Projectiles and Targetting")]
	public GameObject bullet;
	[SerializeField] float fireRate;
	[SerializeField] float coolDownTime;
	[SerializeField] float moveSpeed, distanceToStopMoving;
	private bool inRange;

	public BoatCombat1 ship;
	private DetectShipTrigger detectShipTrigger;
    private bool oilSlicked = false;
    private bool slowedDown = false;

	[Header("Detect the ship before it moves")]
	public bool shipInRange;

	// Use this for initialization
	void Start()
	{
		//for detecting ship before moving
		shipInRange = false;

		maxHealth = 100;
		currentHealth = maxHealth;

		fireRate = 0f;
		coolDownTime = 3f;

		moveSpeed = 3.0f;
		//distanceToStopMoving = 150.0f;

		ship = FindObjectOfType<BoatCombat1>();

		detectShipTrigger = GetComponentInChildren<DetectShipTrigger>();

		InvokeRepeating("FindShipTarget", 0, 1f);
	}

	// Update is called once per frame
	void Update()
	{
		if (shipInRange) MoveToShip();
		if (detectShipTrigger.shipDetected)
		{
			FireRate();
			Shoot();
		}
	}

	void MoveToShip()
	{
		if (distanceToStopMoving < (ship.transform.position - transform.position).sqrMagnitude)
			transform.position = Vector3.MoveTowards(transform.position, closestShipSection.transform.position, moveSpeed * Time.deltaTime);
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
			GameObject enemyBullet = Instantiate(bullet, transform.position, Quaternion.identity);
			enemyBullet.GetComponent<EnemyBulletScript>().moveDirection = (closestShipSection.transform.position - transform.position).normalized;
		}
	}


	//------------------------------------------
	//health related stuff

	public void Health(float damageTaken)
	{
		currentHealth -= damageTaken;
		HealthUi();
		if (currentHealth <= 0f)
		{
			Destroy(gameObject);
		}
	}

	void HealthUi()
	{
		healthBar.fillAmount = currentHealth / maxHealth;
	}
	

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

}