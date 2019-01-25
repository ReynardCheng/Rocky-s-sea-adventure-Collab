﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour {

	[Header("Shooting")]
	public Rigidbody projectile; //reference for projectile
	public float atkRate; // Attack rate

	[Header("Enemy Detection")]
	public Transform TargetingEnemy;
	public List<GameObject> EnemiesInRange;

	[Header("CannonSouns")]
	private AudioSource SoundFromCannon;

	[Header("Health")]
	public int health;
	private int maxHealth;
	public Image healthbar;
	public GameObject smokeEffect;

	[Header("Parent")]
	public GameObject parent;

	public cannonTypes cannonType;

	// Use this for initialization
	void Start () {

		health = 50;
		maxHealth = health;

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
		atkRate = 2f; // Attack cooldown time
		//BulletPos = gun.transform.position;

        switch (cannonType)
        {
            case cannonTypes.normal:
                projectile.GetComponent<BulletFire>().target = TargetingEnemy; //set the target/path for bullets to fly to in a straight line... will want to edit this later on as bullets act like a moving missile.
                Instantiate(projectile, transform.position, transform.rotation);
                break;
            case cannonTypes.aoe:
                projectile.GetComponent<AoeFire>().target = TargetingEnemy; 
                Instantiate(projectile, transform.position, transform.rotation);
                break;
            case cannonTypes.oilSlick:
                projectile.GetComponent<OilSlickFire>().target = TargetingEnemy;
                Instantiate(projectile, transform.position, transform.rotation);
                break;
            default:
                projectile.GetComponent<BulletFire>().target = TargetingEnemy;
                Instantiate(projectile, transform.position, transform.rotation);
                break;

        }
		//SoundFromCannon.Play();
		//nextAtk = Time.time + atkRate;
	}

	public void damageCannon(int damageToTake)
	{
		health -= damageToTake;
		if (health <= 0)
		{
			Destroy(gameObject.transform.parent.gameObject);
		} else {
			UpdateCannonUI();

			if (health <= 30){
				smokeEffect.SetActive(true);
			} else {
				smokeEffect.SetActive(false);
			}
		}
	}

	public void UpdateCannonUI(){
		healthbar.fillAmount = (float) health / maxHealth;
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
