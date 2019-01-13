using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	[Header("Shooting")]
	public Rigidbody projectile; //reference for projectile
	public float atkRate; // Attack rate

	[Header("Enemy Detection")]
	public int speed = 10;
	public Transform TargetingEnemy;
	public List<GameObject> EnemiesInRange;

	[Header("CannonSouns")]
	private AudioSource SoundFromCannon;

	[Header("Health")]
	public int Health;

	// Use this for initialization
	void Start () {

		SoundFromCannon = GetComponent<AudioSource>();

		EnemiesInRange = new List<GameObject>(); //EnemiesInRange = list of enemy targets in the range of the cannon
												 //SphereCollider Range = gameObject.GetComponent<SphereCollider>();
	}

	// Update is called once per frame
	void Update () {

		TargetEnemy();

	}

	void TargetEnemy()
	{
		atkRate -= Time.deltaTime;

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

		if (target != null)
		{
			if (atkRate <= 0)
			{
				NewShoot();
			}

			Vector3 direction = transform.position - target.transform.position; //finding the direction to nearest enemy

			float step = speed * Time.deltaTime;

		}

	}

	private void NewShoot()
	{
		atkRate = 2f; // Attack cooldown time

		//BulletPos = gun.transform.position;
		Instantiate(projectile, transform.position, transform.rotation);
		projectile.GetComponent<BulletFire>().target = TargetingEnemy; //set the target/path for bullets to fly to in a straight line... will want to edit this later on as bullets act like a moving missile.
		SoundFromCannon.Play();
		//nextAtk = Time.time + atkRate;
	}

	public void damageCannon(int damageToTake)
	{
		Health -= damageToTake;
	}

	void OnTriggerEnter(Collider other) //Add enemy to list of targets when in range of cannon
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemiesInRange.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) //Remove enemy from target list when it leaves range
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemiesInRange.Remove(other.gameObject);
		}
	}

}
