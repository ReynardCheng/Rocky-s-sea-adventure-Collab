using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public Text winLoseText;

	public RectTransform bckGroundRecttransform;
	float rctTransformY;
	public bool gameEnded;
	public bool lose;

	public float rate;
	

	// Use this for initialization
	void Start () {
		rctTransformY = bckGroundRecttransform.sizeDelta.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (gameEnded)
		{
			if(bckGroundRecttransform.sizeDelta.y < -1) winAnimation();
		}

	}
	void winAnimation()
	{
		if (!lose) winLoseText.text = ("You Win");
		else if (lose) winLoseText.text = ("You Lose");
		rctTransformY += Time.deltaTime * rate;
		bckGroundRecttransform.sizeDelta = new Vector2(bckGroundRecttransform.sizeDelta.x, rctTransformY);

		//rctTransformY += Time.deltaTime * rate;
		////increase the scale such that it would look like an animation
		//bckGroundRecttransform.localScale = new Vector3(1, rctTransformY, 1);
	}
}
