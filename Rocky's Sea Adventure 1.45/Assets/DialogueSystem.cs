using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    /// <summary>
    /// This script is the dialogue system etc
    /// I don't 100% get the script either so avoid editing the script unless you know what you're touching -ly
    /// </summary>

    public static DialogueSystem instance;
    public ELEMENTS elements;

    public float textSpeed;

    void Awake()
    {
        instance = this;
    }



    /// <summary>
    /// Say something and show it on the speech box.
    /// </summary>
    public void Say(string speech, string speaker = "")
    {
        StopSpeaking();

        speaking = StartCoroutine(Speaking(speech, false, speaker));
    }


    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput = false;

    public string targetSpeech = "";
    Coroutine speaking = null;
    IEnumerator Speaking(string speech, bool additive, string speaker = "")
    {
        //speechPanel.SetActive(true);
        targetSpeech = speech;

        if (!additive)
            speechText.text = "";
        else
            targetSpeech = speechText.text + targetSpeech;

        speakerNameText.text = DetermineSpeaker(speaker);//temporary

        isWaitingForUserInput = false;

        while (speechText.text != targetSpeech)
        {
            speechText.text += targetSpeech[speechText.text.Length];
            yield return new WaitForSeconds(textSpeed);
        }

        //text finished
        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
            yield return new WaitForEndOfFrame();

        StopSpeaking();
    }

    string DetermineSpeaker(string s)
    {
        string retVal = speakerNameText.text;//default return is the current name
        if (s != speakerNameText.text && s != "")
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;

        return retVal;
    }

    [System.Serializable]
    public class ELEMENTS
    {
        /// <summary>
        /// The main panel containing all dialogue related elements on the UI
        /// </summary>
        //public GameObject speechPanel;
        public Text speakerNameText;
        public Text speechText;
    }
    //public GameObject speechPanel { get { return elements.speechPanel; } }
    public Text speakerNameText { get { return elements.speakerNameText; } }
    public Text speechText { get { return elements.speechText; } }
}
