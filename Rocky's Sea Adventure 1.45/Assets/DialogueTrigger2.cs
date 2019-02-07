﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger2 : MonoBehaviour {

    public TutorialController TutControl;

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
            TutControl.ProceedScript4 = true;
            TutControl.Script4.SetActive(true);
            TutControl.Textboxes.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
