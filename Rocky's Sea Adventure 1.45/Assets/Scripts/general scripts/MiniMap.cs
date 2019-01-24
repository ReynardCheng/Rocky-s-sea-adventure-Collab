using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
	public Transform player;
	bool revertFogState = false;

	private void LateUpdate()
	{
		Vector3 newPosition = player.position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;

		//transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
	}

	void OnPreRender()
	{
		revertFogState = RenderSettings.fog;
		RenderSettings.fog = false;
	}

	void OnPostRender()
	{
		RenderSettings.fog = revertFogState;
	}
}
