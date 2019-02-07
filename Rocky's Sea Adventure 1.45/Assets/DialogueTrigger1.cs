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
            TutControl.ProceedScript3 = true;
            TutControl.Textboxes.SetActive(true);
            TutControl.Text.text = "The resources are right in front!";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
