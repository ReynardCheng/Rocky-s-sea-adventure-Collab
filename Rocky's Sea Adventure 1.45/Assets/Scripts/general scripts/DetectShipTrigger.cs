﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectShipTrigger : MonoBehaviour {

    public bool shipDetected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            shipDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ship")
        {
            shipDetected = false;
        }
    }
}
