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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            //ram dmg when hit rocks
            BoatCombat1 theBoatCombat = other.GetComponentInParent<BoatCombat1>();
            BoatController boatControl = other.GetComponentInParent<BoatController>();
            theBoatCombat.DamageShip(boatControl.RamDamageToShip * boatControl.moveFactor);
            print("damage ship:" + boatControl.RamDamageToShip * boatControl.moveFactor);
			other.gameObject.GetComponentInParent<BoatController>().movementFactor = 0;
			other.gameObject.GetComponentInParent<BoatController>().hitWall = true;
		}

    }

 //   private void OnTriggerStay(Collider other)
	//{
	//	if (other.gameObject.layer == 13)	
	//	{
            
	//		other.gameObject.GetComponentInParent<BoatController>().movementFactor = 0;
	//		other.gameObject.GetComponentInParent<BoatController>().hitWall = true;
	//	}
	//}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 13)
		{
			other.gameObject.GetComponentInParent<BoatController>().hitWall = false;
		}
	}

}
