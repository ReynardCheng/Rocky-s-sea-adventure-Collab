using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

	public Vector3 moveDirection;
	// Update is called once per frame
	void Update () {
		transform.Translate(moveDirection * 20, Space.World);
	}
}
