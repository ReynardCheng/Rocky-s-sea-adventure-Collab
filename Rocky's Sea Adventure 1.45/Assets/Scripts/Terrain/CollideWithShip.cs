using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithShip : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == 13)
		{
			other.GetComponentInParent<BoatController>().movementFactor = 0;
			other.gameObject.GetComponentInParent<BoatController>().hitWall = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 13)
		{
			other.gameObject.GetComponentInParent<BoatController>().hitWall = false;
		}
	}

}
