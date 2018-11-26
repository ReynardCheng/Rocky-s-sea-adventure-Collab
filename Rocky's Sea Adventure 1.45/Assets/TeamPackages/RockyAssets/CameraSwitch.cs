using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {

    public Transform playerViewPos;     //Transform of camera when focused on player
    public Transform shipViewPos;       //Transform of camera when focused on ship
    public GameObject ship;

	[SerializeField] private bool shipView;      //This checks what view camera is in right now (ship/player)
    public float switchSpeed;   //How fast the switch will be. The higher the speed, the quicker the switch.

    private bool switching;

	CharacterMovement chMovement;
	BoatMovement theBoat;

	private void Start()
	{
		theBoat = FindObjectOfType<BoatMovement>();
		chMovement = FindObjectOfType<CharacterMovement>();
		shipView = false;
		switching = false; 
		switchSpeed = 0.1f;
	}

	// Update is called once per frame
	void Update () {

		if (chMovement.canControlShip) CameraSwitching();

		CameraParent();
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
			}
			else
			{
				switching = true;
				shipView = false;
				StartCoroutine(SwitchView(playerViewPos));
				transform.parent = null;
			}
		}
	}

	void CameraParent()
	{
		transform.parent = (shipView) ? transform.parent = ship.transform : chMovement.transform;
	}

    IEnumerator SwitchView(Transform view)
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

		if (view == playerViewPos)
		{
			chMovement.fpsController.controllingShip = false;
			theBoat.controllingBoat = false;
			//transform.parent = (transform.position == playerViewPos.transform.position) ? transform.parent = chMovement.transform : transform.parent = null;
		}
		if (view == shipViewPos)
		{
			theBoat.controllingBoat = true;
		}
	}
}

//lerp(a, b, t) = a + (b - a)*t
// ongoing = lerp(initial, target, (start_time + time.time())/speed)