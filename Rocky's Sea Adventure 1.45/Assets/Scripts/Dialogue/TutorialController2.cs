using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController2 : MonoBehaviour {

    /// <summary>
    /// Script to control what happens in each part of tut
    /// </summary>


    DialogueSystem DialogueSys;
    public static int sentenceNumber; // Used to check sentence number
    public GameObject FixedCamera;
    public CharacterMovement Player;
    public GameObject Textboxes;
    public Text Text;
    public GameObject DialogueCanvas;

    public static bool Tutorial = true;

    [Header("Script")]

    public GameObject Script1;
    public bool OnScript1;




    // Use this for initialization
    void Start () {
     
        DialogueSys = FindObjectOfType<DialogueSystem>();
        Player = FindObjectOfType<CharacterMovement>();
        Player.canMove = false;
        OnScript1 = true;

        Tutorial = true;

    }
	
	// Update is called once per frame
	void Update () {

        /// *****************
        /// Starting Dialogue
        /// *****************
        if (OnScript1 == true)
        {
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 15)
            {

                DialogueSys.isWaitingForUserInput = false;
                Player.canMove = true;
                Textboxes.SetActive(false);
                FixedCamera.SetActive(false);
                OnScript1 = false;
                sentenceNumber = 0;
                Tutorial = false;
                DialogueCanvas.SetActive(false);
            }

        }

       
        
    }

    public void SkipTutorial()
    {
        Player.canMove = true;
        DialogueScript.canContinueDialogue = false;
        Textboxes.SetActive(false);
        FixedCamera.SetActive(false);
        OnScript1 = false;
        Tutorial = false;
        DialogueCanvas.SetActive(false);
    }



}
