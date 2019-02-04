using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour {

	CharacterMovement thecharacter;
	public Image blackScreen;
	float lerpRate;

	private void Start()
	{
		thecharacter = FindObjectOfType<CharacterMovement>();
	}

	private void Update()
	{
		lerpRate += Time.deltaTime / 5;
	}

	public void EndCutScene()
	{

	}
	public IEnumerator EndScene()
	{

		blackScreen.gameObject.SetActive(true);
		thecharacter.crRunning = true;
		lerpRate = 0;
		Color designatedColor = Color.black;

		while (blackScreen.color != designatedColor)
		{
			blackScreen.color = Color.Lerp(blackScreen.color, designatedColor, lerpRate);
			yield return null;
		}


		if (designatedColor == Color.black)
			designatedColor = Color.clear;

		while (blackScreen.color != designatedColor)
		{
			blackScreen.color = Color.Lerp(blackScreen.color, designatedColor, lerpRate);
			yield return null;
		}

		blackScreen.gameObject.SetActive(false);
		thecharacter.crRunning = false;

	}

}
