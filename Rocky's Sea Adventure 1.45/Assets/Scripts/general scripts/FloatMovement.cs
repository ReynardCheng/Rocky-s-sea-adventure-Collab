using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMovement: MonoBehaviour {

	[Header("For floating on water : variables")]
	[Space]
	private Transform seaPlane;
	private Cloth planeCloth;
	[SerializeField] private int closestVertexIndex = -1;

	// Use this for initialization
	void Start () {
		seaPlane = GameObject.Find("Sea").transform;
		planeCloth = seaPlane.GetComponent<Cloth>();
	}
	
	// Update is called once per frame
	void Update () {

		GetClosestVertex();

	}

	void GetClosestVertex()
	{
		for (int i = 0; i < planeCloth.vertices.Length; i++)
		{
			if (closestVertexIndex == -1)
			{
				closestVertexIndex = i;
			}

			float distance = Vector3.Distance(planeCloth.vertices[i],transform.position);
			float closestDistance = Vector3.Distance(planeCloth.vertices[closestVertexIndex], transform.position);

			if (distance < closestDistance)
			{
				closestVertexIndex = i;
			}
		}

		transform.localPosition = new Vector3(
			transform.localPosition.x,
			planeCloth.vertices[closestVertexIndex].y / 40,
			transform.localPosition.z
			);
	}
}
