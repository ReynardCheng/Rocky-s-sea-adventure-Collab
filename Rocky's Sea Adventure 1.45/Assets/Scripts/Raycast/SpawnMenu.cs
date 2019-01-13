﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMenu : MonoBehaviour {

	public GameObject menuToSpawn;
	public Transform hitPosition;

	public Vector3 cannonFace;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MenuPosition()
	{
		Transform menuParent = this.transform;
		RaycastToWorld.menuSpawned = true;

		menuToSpawn.transform.position = hitPosition.position;
		Instantiate(menuToSpawn,hitPosition,true);
		
		print("positionChanged");
	}

}
