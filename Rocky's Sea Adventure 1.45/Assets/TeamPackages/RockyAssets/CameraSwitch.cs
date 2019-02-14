using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {

    public Transform playerViewPos;     //Transform of camera when focused on player
    public Transform shipViewPos;       //Transform of camera when focused on ship
    public GameObject ship;

	[SerializeField] private bool shipView;      //This checks what view camera is in right now (ship/player)
    public float switchSpeed;   //How fast the switch will be. The higher the speed, the quicker the switch.

    public bool switching;

    public bool locked;

	CharacterMovement chMovement;
	BoatController theBoat;
    public GameObject shipMastSharedMaterial;
    public GameObject shipFlagsSharedMaterial;

    [Header("For third person rotating")]
	public float turnSpeed;
	public Transform player;

	public Vector3 offset;

	// have to use this for reference because it is from a namespace
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

	private void Start()
	{

		//getting components
		//playerViewPos = CharacterMovement.characterPos.Find("Player View Pos");

		transform.position = playerViewPos.position;
		fpsController = FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
		theBoat = FindObjectOfType<BoatController>();

		//shipViewPos = theBoat.transform.Find("Ship View Position");

		chMovement = FindObjectOfType<CharacterMovement>();
		player = chMovement.transform;

		// setting variables
		shipView = false;
		switching = false; 
		switchSpeed = 0.1f;
	}

	// Update is called once per frame
	void Update () {

		if (chMovement.gameStart)
		{
			if (!locked)
			{
				if (chMovement.canControlShip) CameraSwitching();

				playerViewPos.position = player.position + offset;
				playerViewPos.LookAt(player.position);

				if (!fpsController.controllingShip)
				{
					if (!switching) CameraRotate();
				}
			}

			//if (shipView) theBoat.controllingBoat = true;
			CameraParent();

			//sets the cursor active when the menu is opened
			if (BuildCannonManager.menuSpawned) Cursor.visible = true;
		}
	}

	void CameraSwitching()
	{
		if (Input.GetKeyDown(KeyCode.E) && !switching)
		{
			if (!shipView)
			{
				switching = true;
				shipView = true;
				StartCoroutine(SwitchView(shipViewPos));
				transform.parent = ship.transform;
				fpsController.controllingShip = true;
			}
			else
			{
				switching = true;
				shipView = false;
				StartCoroutine(SwitchView(playerViewPos));
				transform.parent = null;
				//fpsController.controllingShip = false;
			}
		}
	}

	void CameraParent()
	{
		transform.parent = (shipView) ? transform.parent = ship.transform : transform.parent = null;
	}

    public IEnumerator SwitchView(Transform view)
    {

        float fractionLerped = 0f;   //Declaring variable for lerping. This is the fraction of how much of the switch is completed.


        while((transform.position != view.transform.position) && (transform.rotation != view.transform.rotation))   //while switch isnt finished yet...
        {
            fractionLerped += Time.deltaTime * switchSpeed;

			transform.position = Vector3.Lerp(transform.position, view.transform.position, fractionLerped);     //These 2 lines do the actual moving of position and rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, view.transform.rotation, fractionLerped);
            yield return null;
        }
        switching = false;

        if (!locked)
        {
            if (view == playerViewPos)
            {
                shipMastSharedMaterial.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                shipFlagsSharedMaterial.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                fpsController.controllingShip = false;
                theBoat.controllingBoat = false;
                //transform.parent = (transform.position == playerViewPos.transform.position) ? transform.parent = chMovement.transform : transform.parent = null;
            }
            if (view == shipViewPos)
            {
                shipMastSharedMaterial.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                shipFlagsSharedMaterial.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                theBoat.controllingBoat = true;
            }
        }
	}


    void CameraRotate()
    {
		if (!LevelManager.theLevelManager.gamePaused)
		{
			if (!fpsController.controllingShip)
			{
				float nextStep = 2;
				float rate = 10f + nextStep;
				offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
				// transform.position = player.position + offset;
				Vector3 posToLerp = player.position + offset;
				transform.position = Vector3.Lerp(transform.position, posToLerp, Time.deltaTime * rate);
				transform.LookAt(player.position);
				Cursor.visible = false;
			}
		}
    }
}

//lerp(a, b, t) = a + (b - a)*t
// ongoing = lerp(initial, target, (start_time + time.time())/speed)