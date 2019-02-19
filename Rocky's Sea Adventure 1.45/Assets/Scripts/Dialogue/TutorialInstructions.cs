using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstructions : MonoBehaviour {
    /// <summary>
    /// Using for obstructive text
    /// </summary>

    public BuildCannonManager buildCannonScript;
    public BoatController BoatControls;
    public CharacterMovement CharacterMove;
    public static int sentenceCount;
    public static int count = 0;

    [Header("Obstructive Text Stuff")]
    public GameObject ObstructTextbox;
    public Text ObstructiveInstruction;


    [Header("Instruct to steer")]
    public bool haventSteerBefore; //set to false when steering (need to use boatcontroller)
    public bool haventEnteredWheelRange; //set to false when onTriggerEnter collider
    public bool text1;
    public bool text2;

    [Header("Instruct to build")]
    public bool tutorialCannons = false;


    // Use this for initialization
    void Start () {
        haventSteerBefore = true;
        haventEnteredWheelRange = true;
        ObstructiveInstruction.text = "Go to the steering wheel using [WASD]";

        buildCannonScript = FindObjectOfType<BuildCannonManager>();
        buildCannonScript.enabled = false; //cannot build cannons yet, to enable when its time to build

        BoatControls = FindObjectOfType<BoatController>();
        CharacterMove = FindObjectOfType<CharacterMovement>();

	}

    public string[] instructText = new string[]
     {
        "Instruction",

    };
 


    // Update is called once per frame
    void Update () {

      //  sentenceCount = count;

        /// *****************
        /// Navi instruction
        /// ******************
        if (CharacterMove.canControlShip == true)
        {
            haventEnteredWheelRange = false; //set to false when player enters trigger range
            text1 = true;
        }

        if (BoatControls.controllingBoat == true)
        {
            haventSteerBefore = false; //set to false when player starts controlling boat
        }

        if ( count ==1)
        {
            ContinueText(instructText[count]); //call ContinueText with instructText[]
            count = 1; //change count for new instruction
            print("count:" + count);
         
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if ( count <= 2)
            {

                count = 2; //change count for new instruction
                ContinueText(instructText[count]); //call ContinueText with instructText[]
                print("count:" + count);
               
            }
        }


        if (tutorialCannons)
        {
            buildCannonScript.enabled = true; //enable build cannon script
        }



        if (count >= instructText.Length)
        {
            return;
        }
    }

  

    void ContinueText(string instructText)
    {
        string[] instructions = instructText.Split(':');
        string popUpText = instructions[0];
        

        ObstructiveInstruction.text = popUpText;
    }




}
