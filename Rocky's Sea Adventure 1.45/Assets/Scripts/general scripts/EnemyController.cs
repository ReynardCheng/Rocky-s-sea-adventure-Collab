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

	// Use this for initialization
	void Start()
	{
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
		MoveToShip();
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

}