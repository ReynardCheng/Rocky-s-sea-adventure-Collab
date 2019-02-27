using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonController : MonoBehaviour
{
	[System.Serializable]
	public struct CannonVfx
	{
		public AudioClip oilSlickFire;
		public AudioClip cannonFire;
		public AudioClip cannonBallHit;
		public AudioClip aoeHit;
		public AudioClip oilSlickHit;
	}


	[Header("Cannon Sounds")]
	[SerializeField] AudioSource myAudioSource;
	public CannonVfx cannonSounds;

	[Header("Shooting")]
	public Rigidbody projectile; //reference for projectile
	public float atkRate; // Attack rate
	public float slowDebuffTimer = 1;


	[Header("Enemy Detection")]
	public Transform TargetingEnemy;
	public List<GameObject> EnemiesInRange;

	[Header("Health")]
	public int health;
	private int maxHealth;
	public GameObject smokeEffect;

	[Header("Parent")]
	public GameObject parent;

	public cannonTypes cannonType;
	private Animator cannonAnimator;

	// Use this for initialization
	void Start()
	{
		switch (cannonType)
		{
			case (cannonTypes.normal):
				health = 30;
				break;
			case (cannonTypes.aoe):
				health = 50;
				break;
			case (cannonTypes.defence):
				health = 70;
				break;

			default:
				health = 30;
				break;
		}
		maxHealth = health;
		smokeEffect.SetActive(false);

		myAudioSource = GetComponent<AudioSource>();

		EnemiesInRange = new List<GameObject>(); //EnemiesInRange = list of enemy targets in the range of the cannon
												 //SphereCollider Range = gameObject.GetComponent<SphereCollider>();

		cannonAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{

		TargetEnemy();
		if (health <= 0)
		{
			if (cannonType == cannonTypes.defence)
			{
				myAudioSource.PlayOneShot(LevelManager.theLevelManager.defenceCannonDestroyedClip);
			}
			else
			{
				myAudioSource.PlayOneShot(LevelManager.theLevelManager.cannonDestroyedClip);
			}

			Destroy(gameObject.transform.parent.gameObject);
		}
	}

	void TargetEnemy()
	{
		atkRate -= Time.deltaTime;
		slowDebuffTimer -= Time.deltaTime;

		GameObject target = null;

		var distance = Mathf.Infinity;
		Vector3 position = transform.position;

		foreach (GameObject enemy in EnemiesInRange) //For each enemy, find the closest one
		{
			if (enemy == null)
			{
				EnemiesInRange.Remove(enemy);
			}

			var difference = (enemy.transform.position - position);
			var curDistance = difference.sqrMagnitude;
			if (curDistance < distance)
			{
				target = enemy; //setting the target to the closest one
				distance = curDistance;

				TargetingEnemy = target.transform;
			}
		}

		if (target != null && cannonType != cannonTypes.defence)
		{
			if (atkRate <= 0)
			{
				NewShoot();
			}

			Vector3 direction = transform.position - target.transform.position; //finding the direction to nearest enemy
		}

	}

	private void NewShoot()
	{
		if (slowDebuffTimer > 0)
		{
			atkRate = 2f * 2;
		}
		else if (slowDebuffTimer <= 0)
		{
			atkRate = 2f;
		}

		if (cannonAnimator)
		{
			cannonAnimator.SetTrigger("Shoot");
		}

		StartCoroutine(shootProjectile());

		if (cannonType == cannonTypes.oilSlick)
		{
			myAudioSource.PlayOneShot(cannonSounds.oilSlickFire);
		}
		else
		{
			myAudioSource.PlayOneShot(cannonSounds.cannonFire);
		}
	}

	private IEnumerator shootProjectile()
	{
		yield return new WaitForSeconds(10 / 30f);

		switch (cannonType)
		{
			case cannonTypes.normal:
				projectile.GetComponent<BulletFire>().target = TargetingEnemy; //set the target/path for bullets to fly to in a straight line... will want to edit this later on as bullets act like a moving missile.
				projectile.GetComponent<BulletFire>().vfxToPlay = cannonSounds.cannonBallHit;
				Instantiate(projectile, transform.position, transform.rotation);
				break;

			case cannonTypes.aoe:
				projectile.GetComponent<AoeFire>().target = TargetingEnemy;
				projectile.GetComponent<AoeFire>().vfxToPlay = cannonSounds.aoeHit;
				Instantiate(projectile, transform.position, transform.rotation);
				break;

			case cannonTypes.oilSlick:
				projectile.GetComponent<OilSlickFire>().target = TargetingEnemy;
				projectile.GetComponent<OilSlickFire>().vfxToPlay = cannonSounds.oilSlickHit;
				Instantiate(projectile, transform.position, transform.rotation);
				break;

			default:
				projectile.GetComponent<BulletFire>().target = TargetingEnemy;
				projectile.GetComponent<BulletFire>().vfxToPlay = cannonSounds.cannonBallHit;
				Instantiate(projectile, transform.position, transform.rotation);
				break;
		}
	}

	public void damageCannon(int damageToTake)
	{
		health -= damageToTake;

		if (health <= 0)
		{
			if (cannonType == cannonTypes.defence)
			{
				myAudioSource.PlayOneShot(LevelManager.theLevelManager.defenceCannonDestroyedClip);
			}
			else
			{
				myAudioSource.PlayOneShot(LevelManager.theLevelManager.cannonDestroyedClip);
			}

			Destroy(gameObject.transform.parent.gameObject);
		}
		else
		{
			if (cannonType == cannonTypes.defence)
			{
				myAudioSource.PlayOneShot(LevelManager.theLevelManager.defenceCannonDefendingClip);
			}

			if (health <= 30)
			{
				smokeEffect.SetActive(true);
			}
			else
			{
				smokeEffect.SetActive(false);
			}
		}
	}

	void OnTriggerEnter(Collider other) //Add enemy to list of targets when in range of cannon
	{
		if (other.gameObject.tag == "Enemy" || other.tag == "Boss")
		{
			EnemiesInRange.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) //Remove enemy from target list when it leaves range
	{
		if (other.gameObject.tag == "Enemy" || other.tag == "Boss")
		{
			EnemiesInRange.Remove(other.gameObject);
		}
	}

}
