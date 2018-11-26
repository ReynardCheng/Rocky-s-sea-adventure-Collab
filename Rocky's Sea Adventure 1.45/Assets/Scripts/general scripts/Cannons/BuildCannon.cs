﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCannon : MonoBehaviour
{

	public GameObject cannonPrefab;
    public CannonHealth cannonHealth;
    public GameObject linkedCannon;

    private bool canBuild;
	public bool slotTaken; // == true when a cannon is already built in position


	// Use this for initialization
	void Start()
	{
		slotTaken = false;

        cannonHealth = GetComponent<CannonHealth>();
        CheckForCannon();
	}

	// Update is called once per frame
	void Update()
	{
		BuildTower();
	}

	void CheckForCannon()   //Runs at beginning of game to "start" the default cannons
	{
        if (transform.childCount > 0)
        {
            slotTaken = true;
            cannonHealth.SetBeginningHealth(30.0f);
        }
	}

	void BuildTower()
	{
		if (Input.GetKeyDown(KeyCode.E) && canBuild && !slotTaken)
		{
			
			linkedCannon = Instantiate(cannonPrefab, transform.position, cannonPrefab.transform.rotation);
            linkedCannon.transform.Rotate(transform.localRotation.eulerAngles.z, cannonPrefab.transform.rotation.y, cannonPrefab.transform.rotation.z); //Rotates any cannon to face outwards
            linkedCannon.transform.parent = transform.parent.parent; //Sets cannon transform to ship

            cannonHealth.SetBeginningHealth(30.0f);
            

			slotTaken = true;
			print("Spawned");
		}

	}

	void OnTriggerEnter(Collider other) //Add enemy to list of targets when in range of cannon
	{
		if (other.gameObject.tag == "Player")
		{
			canBuild = true;
		}
	}

	void OnTriggerExit(Collider other) //Remove enemy from target list when it leaves range
	{
		if (other.gameObject.tag == "Player")
		{
			canBuild = false;
		}
	}
}
