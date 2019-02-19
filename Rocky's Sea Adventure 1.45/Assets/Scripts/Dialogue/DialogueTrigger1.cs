using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger1 : MonoBehaviour {

    public TutorialController TutControl;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            DialogueScript.onSentence = 0;
            TutControl = FindObjectOfType<TutorialController>();
            TutControl.ProceedScript2 = true;
            TutControl.Script2.SetActive(true);
            TutControl.Textboxes.SetActive(true);
            print(TutControl);
            print("is working");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
