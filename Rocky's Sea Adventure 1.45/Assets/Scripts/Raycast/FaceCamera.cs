using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public Camera mainCam;
    public bool isSlider;

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
        if (!isSlider)
            transform.LookAt(mainCam.transform);
        else
            transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.back, mainCam.transform.rotation * Vector3.down);
    }
}
