using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {

	public int intToSave;
	public string levelToLoad;
	//settle win lose
	myGUI theGui;
	[SerializeField] int highestLevelCleared;

	// Use this for initialization
	void Start () {
		highestLevelCleared = PlayerPrefs.GetInt("HighestLevelCleared");
		theGui = FindObjectOfType<myGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship")
        {
            other.GetComponent<BoatController>().reachedEnd = true;
			theGui.winAnimation();
			if (highestLevelCleared < intToSave)
			{
				PlayerPrefs.SetInt("HighestLevelCleared", intToSave);
				print("Saved");
				other.GetComponent<BoatController>().reachedEnd = true;
				theGui.lose = false;
				theGui.winAnimation();
				//LoadingScreen.theLoadingScreen.loadLevel(levelToLoad);
			}
		}
	}
}
