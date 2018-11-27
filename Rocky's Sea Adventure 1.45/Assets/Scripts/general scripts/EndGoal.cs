using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour {

    public bool levelCleared;
    public GameObject winScreen;

	// Use this for initialization
	void Start () {
        winScreen.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            levelCleared = true;
            winScreen.SetActive(true);

        }
    }
}
