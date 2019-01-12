using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public Camera mainCam;

	// Use this for initialization
	void Start()
	{
		mainCam = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		cameraFace();
	}

	void cameraFace()
	{
		transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.back, mainCam.transform.rotation * Vector3.down);
	}
}
