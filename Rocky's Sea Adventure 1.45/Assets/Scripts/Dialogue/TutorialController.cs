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


    [Header("Mini-map Intro")]
    public GameObject MapHighlight;
    public GameObject CompassHighlight;
    public GameObject Script1;
    public bool OnScript1;
    public bool ProceedScript2;

    [Header("Navigation")]
    public GameObject Script2;

    [Header("Resources")]
    public bool ProceedScript3;
    public GameObject Script3;

    // Use this for initialization
    void Start () {
        DialogueSys = FindObjectOfType<DialogueSystem>();
        Player.canMove = false;
        OnScript1 = true;
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

            if (sentenceNumber == 3)
            {
                MapHighlight.SetActive(true);
            }

            if (sentenceNumber == 5)
            {
                MapHighlight.SetActive(false);
                CompassHighlight.SetActive(true);
            }
            if (sentenceNumber == 6)
            {
                CompassHighlight.SetActive(false);
            }
            if (sentenceNumber == 7)
            {
                DialogueSys.isWaitingForUserInput = false;
                Player.canMove = true;
                //Textboxes.SetActive(false);
                FixedCamera.SetActive(false);
                ProceedScript2 = true;
                Script2.SetActive(true);
                OnScript1 = false;
            }
        }


        /// *****************
        /// Navi intro
        /// ******************
        if (ProceedScript2 == true)
        {
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            //may want to edit to set trigger in front of wheel to pop out dialogue box again
            if (sentenceNumber == 1)
            {
                Destroy(Script1);
            }
            if (sentenceNumber == 5)
            {
                DialogueSys.isWaitingForUserInput = false;
                Script2.SetActive(false);
                Textboxes.SetActive(false);
                ProceedScript2 = false;
            }
        }

        /// *****************
        /// Resource intro
        /// ******************
        if (ProceedScript3 == true)
        {
            sentenceNumber = DialogueScript.onSentence;
            print("Sentence index = " + sentenceNumber);

            if (sentenceNumber == 1)
            {
                Destroy(Script2);
                Player.gameStart = false;
            }
            if (sentenceNumber == 3)
            {
                Player.gameStart = true;
                DialogueSys.isWaitingForUserInput = false;
                Script3.SetActive(false);
                Textboxes.SetActive(false);

            }
        }
    }
}
