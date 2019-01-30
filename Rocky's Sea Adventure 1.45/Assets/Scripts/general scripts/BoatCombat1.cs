using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatCombat1 : MonoBehaviour
{

	public int shipHealth = 100;
    private float shipMaxHP; //Keep this one as float yo

	public GameObject[] cannonHolder;
    private int cannonToDamageIndex;

	private bool canDefend = false;

	[SerializeField] private GameObject shipRightSide, shipLeftSide;

	[SerializeField] private LayerMask cannonMask;
	[SerializeField] private LayerMask cannonSlot;

    [SerializeField] private Slider shipHealthBar;
    [SerializeField] private Text shipHealthText;

    public void Start()
    {
        shipMaxHP = shipHealth;
        shipHealthText.text = shipHealth.ToString();
        shipHealthBar.value = shipMaxHP;
    }

    public void TakeDamage(int damageToTake, GameObject damageLocation)
	{
		//This array contains the cannons that is within
		//the damage radius of enemy projectile that hit the ship
		Collider[] cannonsInRange = Physics.OverlapSphere(damageLocation.transform.position, 5.0f, cannonMask);

		///////////////////////////
		//Finding if ship left side or right side is closer to damagelocation
		bool RightSideCloser;
		var leftDiff = damageLocation.transform.position - shipLeftSide.transform.position;
		var leftDiffSqr = leftDiff.sqrMagnitude;

		var rightDiff = damageLocation.transform.position - shipRightSide.transform.position;
		var rightDiffSqr = rightDiff.sqrMagnitude;

		if (leftDiffSqr > rightDiffSqr)
			RightSideCloser = true;
		else
			RightSideCloser = false;
		////////////////////////////////////////


		//This for loop checks if there are 
		//Currently any defence cannons on the ship,
		//and if they are on the correct side of the ship
		//and are able to protect the ship or its cannons
		for (int i = 0; i < cannonHolder.Length; i++)
		{
			if (cannonHolder[i].GetComponentInChildren<CannonController>() != null)
			{
				if (cannonHolder[i].GetComponentInChildren<CannonController>().cannonType == cannonTypes.defence)
				{
					if (i > 2 && i < 6 && RightSideCloser)
						canDefend = true;
					else if (i < 3 && !RightSideCloser)
						canDefend = true;
					else
						canDefend = false;
				}
			}
		}

		//If there are currently defence cannons on the ship,
		//And damagelocation is the same side of ship as the defence cannon,
		if (canDefend == true)
		{
			print("defending!");
			//Check if the enemy projectile's damage radius can hit at least one cannonslot
			if (Physics.OverlapSphere(damageLocation.transform.position, 5.0f, cannonSlot).Length != 0)
			{
				GameObject cannonToDamage = null;

				//If Ship right side is closer,
				//Damage the defence cannons on the right side.
				if (RightSideCloser)
				{
					for (int i = 3; i < 6; i++)
					{
						if (cannonHolder[i].GetComponentInChildren<CannonController>() != null)
						{
							if (cannonHolder[i].GetComponentInChildren<CannonController>().cannonType == cannonTypes.defence)
							{
								cannonToDamage = cannonHolder[i].GetComponentInChildren<CannonController>().gameObject;
								DamageCannons(cannonToDamage, damageToTake);
							}
						}
					}
				}

				//If Ship left side is closer,
				//Damage the defence cannons on the right side.
				if (!RightSideCloser)
				{
					if (cannonToDamage == null)
					{
						for (int i = 0; i < 3; i++)
						{
							if (cannonHolder[i].GetComponentInChildren<CannonController>() != null)
							{
								if (cannonHolder[i].GetComponentInChildren<CannonController>().cannonType == cannonTypes.defence)
								{
									cannonToDamage = cannonHolder[i].GetComponentInChildren<CannonController>().gameObject;
									DamageCannons(cannonToDamage, damageToTake);
								}
							}
						}
					}
				}

			}
		}
		else if (cannonsInRange.Length == 0)
		{
			print("damageShip!");
			DamageShip(damageToTake);
		}
		else
		{
			print("damagingCannons!");
			//Finding Which Cannon to Damage
			GameObject cannonToDamage = null;
			float distance = Mathf.Infinity;

			foreach (Collider cannon in cannonsInRange)
			{
				float curDistance = (cannon.gameObject.transform.position - damageLocation.transform.position).sqrMagnitude;
				if (curDistance < distance)
					cannonToDamage = cannon.gameObject;
			}

			DamageCannons(cannonToDamage, damageToTake);
		}

	}


	public void DamageShip(int damageToTake)
	{
		shipHealth -= damageToTake;
        shipHealthText.text = shipHealth.ToString();
        shipHealthBar.value = shipHealth / shipMaxHP;
	}

	private void DamageCannons(GameObject cannonToDamage, int damageToTake)
	{
		cannonToDamage.GetComponentInChildren<CannonController>().damageCannon(damageToTake);

		//Restarting bools
		canDefend = false;
	}
}