using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeFog : MonoBehaviour {

	private bool revertFogState = false;


	private void OnPreRender()
	{
		revertFogState = RenderSettings.fog;
		RenderSettings.fog = false;
	}

	private void OnPostRender()
	{
		RenderSettings.fog = revertFogState;
	}
}
