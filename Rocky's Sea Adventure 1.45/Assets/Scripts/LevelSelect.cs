using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

	[Header("Unlock")]
	public int intToUnlock;
	public string stringToGet;
	[SerializeField] int highestLevelCleared;
	
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll();
		highestLevelCleared = PlayerPrefs.GetInt(stringToGet);
	}
	
	// Update is called once per frame
	void Update () {
		LevelStatus();
	}

	void LevelStatus()
	{
		if (highestLevelCleared >= intToUnlock)
		{
			gameObject.GetComponent<Button>().interactable = true;
		}
		else
		{
			gameObject.GetComponent<Button>().interactable = false;
		}
	}

	public void loadLevel(string levelToLoad)
	{
		LoadingScreen.theLoadingScreen.loadLevel(levelToLoad);
	}
}
