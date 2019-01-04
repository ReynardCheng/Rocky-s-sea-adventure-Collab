using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReynardsNameSpace
{
	public class CharacterRotate : MonoBehaviour
	{
		float rotation;
		

		private void Update()
		{
			Rotate();
		}
		void Rotate()
		{
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
			{
				rotation = -135f;
			}
			else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
			{
				rotation = -45f;
			}
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
			{
				rotation = 45f;
			}
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
			{
				rotation = -225f;
			}
			else if (Input.GetKey(KeyCode.W))
			{
				rotation = -90f;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				rotation = 90f;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				rotation = 0f;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				rotation = -180f;
			}

			Quaternion rotationToLerp = Quaternion.Euler(0, rotation, 0);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotationToLerp, 1f);

		}
	}

	
}

