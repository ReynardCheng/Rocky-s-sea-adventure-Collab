using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour {

    public GameObject ShipUIMenu;
    public GameObject UIStuff;
    public GameObject CannonUI;
    public CharacterMovement Player;

    [Header("Dialogue Stuff")]
    public GameObject DialogueThing;
    public GameObject FixedCamera;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        //set all the UI stuff false during cutscene
        ShipUIMenu.SetActive(false);
        UIStuff.SetActive(false);
        CannonUI.SetActive(false);
    }

    public void offThisCam()
	{
		gameObject.GetComponent<Camera>().enabled = false;
        Player.gameStart = true;

        //set ui stuff back on 
        ShipUIMenu.SetActive(true);
        UIStuff.SetActive(true);
        CannonUI.SetActive(true);

        //Dialogue lets go
        DialogueThing.SetActive(true);
        FixedCamera.SetActive(true);

    }
}
