using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    /// <summary>
    /// Script to control what happens in each part of tut
    /// </summary>


    DialogueSystem DialogueSys;
    public static int sentenceNumber; // Used to check sentence number
    public GameObject FixedCamera;
    public CharacterMovement Player;
    public GameObject Textboxes;
    public Text Text;
    public GameObject ObstructBox;
    public TutorialInstructions TutInstruct;

    public static bool Tutorial = true;

    [Header("Navigation")]

    public GameObject Script1;
    public bool OnScript1;
    public bool ProceedScript2;

    [Header("Resources")]
    public GameObject Script2;

    [Header("Cannon")]
    public bool ProceedScript3;
    public GameObject Script3;
    public GameObject Trigger;

    [Header("CannonBuild End")]
    public bool ProceedScript4;
    public GameObject Script4;

    [Header("Enemies")]
    public GameObject Script5;
    public bool ProceedScript5;

    [Header("Upgrade")]
    public GameObject Script6;
    public bool ProceedScript6;

    [Header("Boost & Ram")]
    public GameObject Script7;
    public bool ProceedScript7;

    [Header("Adv Enemies")]
    public GameObject Script8;
    public bool ProceedScript8;

    [Header("End")]
    public GameObject Script9;
    public bool ProceedScript9;


    // Use this for initialization
    void Start () {
        TutInstruct = FindObjectOfType<TutorialInstructions>();
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

            if (sentenceNumber == 4)
            {

                ObstructBox.SetActive(true);//test
                DialogueSys.isWaitingForUserInput = false;
                Player.canMove = true;
                Textboxes.SetActive(false);
                FixedCamera.SetActive(false);
                OnScript1 = false;
                sentenceNumber = 0;
            }

        }

        /// *****************
        /// Resource intro
        /// ******************
        if (ProceedScript2 == true)
        {
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);
            

            if (sentenceNumber == 0)
            {
                ObstructBox.SetActive(false);
                Text.text = "There are resources right ahead!";
                Destroy(Script1);
                Player.canMove = false;
            }
            if (sentenceNumber == 5)
            {
                DialogueSys.isWaitingForUserInput = false;
                Script2.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript2 = false;
                Player.canMove = true;
                ObstructBox.SetActive(true);
                if (TutorialInstructions.count <= 2) TutorialInstructions.count = 3;
            }
        }

        /// *****************
        /// Cannon Build
        /// ******************
        if (ProceedScript3 == true)
        {
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                Player.canMove = false;
                ObstructBox.SetActive(false);
                Destroy(Script2);
                Text.text = "Let’s build a cannon before moving ahead.";
				
            }
            if (sentenceNumber == 4)
            {
                
                DialogueSys.isWaitingForUserInput = false;
                Script3.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript3 = false;
                Player.canMove = true;
                ObstructBox.SetActive(true);
                Trigger.SetActive(true);
                if (TutorialInstructions.count <= 3) TutorialInstructions.count = 4;
            }
        }

        /// *****************
        /// Cannon intro end
        /// ******************
        if (ProceedScript4 == true)
        {
           
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                ObstructBox.SetActive(false);
                Destroy(Script3);
                Text.text = "You have a cannon now!";
            }
            if (sentenceNumber == 5)
            {

                DialogueSys.isWaitingForUserInput = false;
                Script4.SetActive(false);
                Player.canMove = true;
                Textboxes.SetActive(false);
                ProceedScript4 = false;
            }
        }


        /// *****************
        /// Enemies intro
        /// ******************
        if (ProceedScript5 == true)
        {

            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                ObstructBox.SetActive(false);
                Destroy(Script4);
                Text.text = "There's an enemy!";
                Player.canMove = false;
            }
            if (sentenceNumber == 5)
            {
                Player.canMove = true;
                DialogueSys.isWaitingForUserInput = false;
                Script5.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript5 = false;
            }
        }

        /// *****************
        /// Upgrade Cannons
        /// ******************
        if (ProceedScript6 == true)
        {

            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                Player.canMove = false;
                ObstructBox.SetActive(false);
                Destroy(Script5);
                Text.text = "Phew! That was a close one.";
            }
            if (sentenceNumber == 8)
            {
                Player.canMove = true;
                DialogueSys.isWaitingForUserInput = false;
                Script6.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript6 = false;
                ObstructBox.SetActive(true);
                if (TutorialInstructions.count <= 6) TutorialInstructions.count = 7;
            }
        }

        /// *****************
        /// Boost & ram
        /// ******************
        if (ProceedScript7 == true)
        {

            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                ObstructBox.SetActive(false);
                Destroy(Script6);
                Text.text = "You know, we could just ram the ship into the enemies too!";
            }
            if (sentenceNumber == 3)
            {

                DialogueSys.isWaitingForUserInput = false;
                Script7.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript7 = false;
            }
        }

         /// *****************
         /// Adv enemies
         /// ******************
         if (ProceedScript8 == true)
            {

             sentenceNumber = DialogueScript.onSentence;
             print("Sentence index = " + sentenceNumber);

             if (sentenceNumber == 0)
             {
                Player.canMove = false;
                    Destroy(Script7);
                    Text.text = "Look out! Stronger enemies ahead!";
             }
             if (sentenceNumber == 6)
             {
                Player.canMove = true;
                    DialogueSys.isWaitingForUserInput = false;
                    Script8.SetActive(false);
                    Textboxes.SetActive(false);
                    ProceedScript8 = false;
                    ObstructBox.SetActive(true);
                    if (TutorialInstructions.count <= 7) TutorialInstructions.count = 8;

             }
          }

        /// *****************
        /// End
        /// ******************
        if (ProceedScript9 == true)
        {

            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 0)
            {
                ObstructBox.SetActive(false);
                Destroy(Script8);
                Text.text = "The way out is just right in front of us!";
            }
            if (sentenceNumber == 4)
            {

                DialogueSys.isWaitingForUserInput = false;
                Script9.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript9 = false;
                Tutorial = false;
            }
        }
    }
}
