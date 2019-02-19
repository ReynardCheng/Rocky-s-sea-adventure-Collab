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

    [Header("Obstructive Text Stuff")]
    public GameObject ObstructTextbox;
    public Text ObstructiveInstruction;


    [Header("Instruct to steer")]
    public bool haventSteerBefore; //set to false when steering (need to use boatcontroller)
    public bool haventEnteredWheelRange; //set to false when onTriggerEnter collider

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

	}

    public string[] instructText = new string[]
     {
        "Instruction",

    };
    public int count = 0;


    // Update is called once per frame
    void Update () {

        if (BoatControls.controllingBoat == true)
        {
            haventSteerBefore = false; //set to false when player starts controlling boat
        }

        if (haventSteerBefore == false)
        {
            
            ContinueText(instructText[count]); //call ContinueText with instructText[]
            count = 1; //change count for new instruction
            print("count:" + count);
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
