using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager  {

	// This is to get the controls of player 1 
	public static Vector2 GetP1MoveMent()
	{
		Vector2 v = new Vector2(
			Input.GetAxis("Horizontal(P1)"),
			Input.GetAxis("Vertical(P1)")
			);
		v.Normalize();
		return v;
	}

	// this is to get the controls for the mouse so that later it can help to rotate the camera
	public static Vector2 GetP1Rotation()
	{

		Vector2 v = new Vector2(
			Input.GetAxis("Mouse X (P1)"),
			Input.GetAxis("Mouse Y (P1)")
			);

		// this is to normalise the v vector so that you can get diagonal movement later
		v.Normalize();
		return v;

	}

}
