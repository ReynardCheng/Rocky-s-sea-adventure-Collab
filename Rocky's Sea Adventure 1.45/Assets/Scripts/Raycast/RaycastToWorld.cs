using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastToWorld : MonoBehaviour {
	public static bool menuSpawned;
	public Camera cam;
	SpawnMenu theMenu;
    public LayerMask cannonMask;

	// Use this for initialization
	void Start () {

		menuSpawned = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0) && !menuSpawned)
		{
			RaycastToWorldPos();
		}

	}

	void RaycastToWorldPos()
	{


		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;


		if (Physics.Raycast(ray, out hit,Mathf.Infinity, cannonMask))
		{
			if (hit.collider != null)
			{
				if (hit.collider.tag == "Cannon")
				{
					theMenu = hit.collider.gameObject.GetComponent<SpawnMenu>();
					theMenu.hitPosition = hit.collider.transform;
					theMenu.MenuPosition();
					print("HIt");
				}
				else if (hit.collider.tag == "CannonSlot" )
				{
					if (!hit.collider.GetComponent<BuildCannon>().slotTaken)
					{
						theMenu = hit.collider.gameObject.GetComponent<SpawnMenu>();
						theMenu.hitPosition = hit.collider.transform;
						theMenu.MenuPosition();
						print("HIt SLot	");
					}
				}
			}
		}
	}
}
