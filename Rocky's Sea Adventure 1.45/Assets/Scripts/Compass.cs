using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

	public Transform ship;
	public Transform objective;

	// Use this for initialization
	void Start () {
        ship = FindObjectOfType<BoatController>().transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = (objective.position - ship.position).normalized;
		float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

		transform.eulerAngles = new Vector3(0f, 0f, -angle);
	}
}
