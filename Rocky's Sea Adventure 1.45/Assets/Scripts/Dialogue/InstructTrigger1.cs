using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructTrigger1 : MonoBehaviour {

    public TutorialInstructions TutInstruct;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (TutorialInstructions.count <= 4) TutorialInstructions.count = 5;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
