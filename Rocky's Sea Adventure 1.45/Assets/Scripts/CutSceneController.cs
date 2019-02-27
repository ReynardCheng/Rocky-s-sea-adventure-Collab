using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{

    public bool forFunctions;
    public GameObject bel;

    public GameObject ShipUIMenu;
    public GameObject UIStuff;
    public GameObject CannonUI;
    public CharacterMovement Player;

    [Header("Dialogue Stuff")]
    public GameObject DialogueThing;
    public GameObject FixedCamera;

	Camera mainCam;

    // Update is called once per frame
    void Update()
    {

    }

    private void Start()
    {
		mainCam = Camera.main;
        if (!forFunctions)
        {
            //set all the UI stuff false during cutscene
            ShipUIMenu.SetActive(false);
            UIStuff.SetActive(false);
            CannonUI.SetActive(false);
        }
    }

    public void offThisCam()
    {
        gameObject.GetComponent<Camera>().enabled = false;
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

    public void offAndStartGame()
    {
        CharacterMovement player = FindObjectOfType<CharacterMovement>();
        player.gameStart = true;
        player.canMove = true;
        gameObject.SetActive(false);
    }

    public void SkipAnimation()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.Play("CutSceneAnimation1", 0, 1f);

        Animator fairyAnim = bel.GetComponent<Animator>();
        fairyAnim.Play("TutorialAnimation", 0, 1f);
    }

	// Boss cutscene animation
	void OffMainCamera()
	{
		mainCam.enabled = false;
	}

	public void OnCamera()
	{
		mainCam.enabled = true;
	}
}
