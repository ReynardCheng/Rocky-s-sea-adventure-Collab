using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {

	public int intToSave;
	[SerializeField] int highestLevelCleared;

	// Use this for initialization
	void Start () {
		highestLevelCleared = PlayerPrefs.GetInt("HighestLevelCleared");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship")
		{
			if (highestLevelCleared < intToSave)
			{
				PlayerPrefs.SetInt("HighestLevelCleared", intToSave);
				print("Saved");
			}
		}
	}
}
