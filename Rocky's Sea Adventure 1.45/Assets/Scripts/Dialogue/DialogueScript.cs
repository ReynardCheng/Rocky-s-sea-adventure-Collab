using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    /// <summary>
    /// Attach the script and add dialogue only in the inspector
    /// </summary>

    DialogueSystem dialogueSys;
    public static int onSentence;
    public static bool canContinueDialogue;


    void Start()
    {
        dialogueSys = FindObjectOfType<DialogueSystem>();
        canContinueDialogue = true;
    }
    /// <summary>
    /// Honestly, the string component is the only one you need to know
    /// PLEASE USE THE UNITY TO ADD SENTENCES
    /// keep track of the sentence number
    /// After any sentence add : to seperate dialogue text from name
    /// You don't need to put the name on every line, just when changing name
    /// </summary>

    public string[] s = new string[]
    {
        "Text:Name",

    };


    public int index = 0;

    void Update()
    {
        onSentence = index;


        if (Input.GetMouseButtonDown(0))
        {
            //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //collider box to be attached to gameobject

            //if (hit.collider != null && canContinueDialogue == true)

                if (canContinueDialogue == true)
                {


                    if (!dialogueSys.isSpeaking || dialogueSys.isWaitingForUserInput)
                    {
                        if (index >= s.Length)
                        {
                            return;
                        }
                        Say(s[index]);
                        index++;
                        onSentence++;
                    }


                }
        }
    }



    void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        dialogueSys.Say(speech, speaker);
    }



}
