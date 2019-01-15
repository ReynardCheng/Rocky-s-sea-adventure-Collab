using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCannon : MonoBehaviour
{

	public GameObject cannonPrefab;
	//    public CannonHealth cannonHealth;
	public GameObject linkedCannon;

    private bool canBuild;
	public bool slotTaken; // == true when a cannon is already built in position

	Vector3 face;

	// Use this for initialization
	void Start()
	{
		slotTaken = false;

       // cannonHealth = GetComponent<CannonHealth>();
        //CheckForCannon();
	}

	// Update is called once per frame
	void Update()
	{
		//BuildTower();
	}

	void CheckForCannon()   //Runs at beginning of game to "start" the default cannons
	{
        if (transform.childCount > 0)
        {
            slotTaken = true;
           // cannonHealth.SetBeginningHealth(30.0f);
        }
	}

    

	public void BuildTowerButton()
	{
		//to get thet settings on where the cannon should face
		face = GetComponentInParent<SpawnMenu>().cannonFace;


		linkedCannon = Instantiate(cannonPrefab, transform.position, cannonPrefab.transform.rotation);

		linkedCannon.transform.parent = transform.parent;

		linkedCannon.transform.localEulerAngles = face;

		linkedCannon.GetComponentInParent<BuildCannon>().slotTaken = true;

		RaycastToWorld.menuSpawned = false;
		print(face);
		Destroy(gameObject);

	}
	//void BuildTower()
	//{
	//    if (Input.GetKeyDown(KeyCode.E) && canBuild && !slotTaken)
	//    {

	//        linkedCannon = Instantiate(cannonPrefab, transform.position, cannonPrefab.transform.rotation);
	//        print(transform.localRotation.eulerAngles.y);
	//        linkedCannon.transform.Rotate(180, transform.localRotation.eulerAngles.y, 0); //Rotates any cannon to face outwards
	//        linkedCannon.transform.parent = transform.parent.parent; //Sets cannon transform to ship

	//        cannonHealth.SetBeginningHealth(30.0f, linkedCannon);


	//        slotTaken = true;
	//    }

	//}

	void OnTriggerEnter(Collider other) //Add enemy to list of targets when in range of cannon
	{
		if (other.gameObject.tag == "Player")
		{
			canBuild = true;
		}
	}

	void OnTriggerExit(Collider other) //Remove enemy from target list when it leaves range
	{
		if (other.gameObject.tag == "Player")
		{
			canBuild = false;
		}
	}
}
