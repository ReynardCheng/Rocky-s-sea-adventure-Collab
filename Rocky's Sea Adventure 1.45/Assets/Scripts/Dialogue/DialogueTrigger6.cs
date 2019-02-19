using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger6 : MonoBehaviour {
    public TutorialController TutControl;
    public TutorialInstructions TutInstruct;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            DialogueScript.onSentence = 0;
            TutControl.ProceedScript8 = true;
            TutControl.Script8.SetActive(true);
            TutControl.Textboxes.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
