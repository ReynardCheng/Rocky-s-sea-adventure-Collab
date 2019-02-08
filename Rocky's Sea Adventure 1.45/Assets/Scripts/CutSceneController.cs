using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void offThisCam()
	{
		gameObject.GetComponent<Camera>().enabled = false;
<<<<<<< HEAD
        Player.gameStart = true;

        //set ui stuff back on 
        ShipUIMenu.SetActive(true);
        UIStuff.SetActive(true);
        CannonUI.SetActive(true);
        gameObject.SetActive(false);

        //Dialogue lets go
        DialogueThing.SetActive(true);
        FixedCamera.SetActive(true);
        

    }
=======
	}
>>>>>>> b56eb36cfb8207e459fd057428a5245808aa20fa
}
