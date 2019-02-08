using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatCombat1 : MonoBehaviour
{

	public int shipHealth = 100;
    public float shipMaxHP; //Keep this one as float yo

	public GameObject[] cannonHolder;
    private int cannonToDamageIndex;
    private EnemyBulletScript.bulletType bulletType;
    
    [SerializeField] private float protectionRadius;
	[SerializeField] private GameObject shipRightSide, shipLeftSide;

	[SerializeField] private LayerMask cannonMask;
	[SerializeField] private LayerMask cannonSlot;
    [SerializeField] private LayerMask bulletMask;

    [SerializeField] private Image shipHealthBar;
    [SerializeField] private Text shipHealthText;

    public void Start()
    {
        shipMaxHP = shipHealth;
	
		shipHealthBar.fillAmount = shipHealth / shipMaxHP;
		shipHealthText.text = shipHealth.ToString();
		
    }

    public void TakeDamage(int damageToTake, GameObject damageLocation)
	{
        bulletType = damageLocation.GetComponent<EnemyBulletScript>().bulletState;

		//This array contains the cannons that is within
		//the damage radius of enemy projectile that hit the ship
		Collider[] cannonsInRange = Physics.OverlapSphere(damageLocation.transform.position, 2, cannonMask);
		print("Cannons in Range:" + cannonsInRange.Length);
		///////////////////////////
		//Finding if ship left side or right side is closer to damagelocation
		//bool RightSideCloser;
		//var leftDiff = damageLocation.transform.position - shipLeftSide.transform.position;
		//var leftDiffSqr = leftDiff.sqrMagnitude;

		//var rightDiff = damageLocation.transform.position - shipRightSide.transform.position;
		//var rightDiffSqr = rightDiff.sqrMagnitude;

		//if (leftDiffSqr > rightDiffSqr)
		//	RightSideCloser = true;
		//else
		//	RightSideCloser = false;
		////////////////////////////////////////


		//This for loop checks if there are 
		//Currently any defence cannons on the ship,
        //If yes, do an overlapsphere to check for a bulelt within defence cannon's defensive range.
        //If bullet is inside, damage the defence cannon and stop the entire method.
		for (int i = 0; i < cannonHolder.Length; i++)
		{
			if (cannonHolder[i].GetComponentInChildren<CannonController>() != null)
			{
				if (cannonHolder[i].GetComponentInChildren<CannonController>().cannonType == cannonTypes.defence)
				{
                    if ((cannonHolder[i].transform.position - damageLocation.transform.position).magnitude < protectionRadius)
                    {
                        DamageCannons(cannonHolder[i].gameObject, damageToTake);
                        return;
                    }
                    
					//if (i > 2 && i < 6 && RightSideCloser)
					//	canDefend = true;
					//else if (i < 3 && !RightSideCloser)
					//	canDefend = true;
					//else
					//	canDefend = false;
				}
			}
		}


		if (cannonsInRange.Length == 0)
		{
			DamageShip(damageToTake);
		}
		else //There is a cannon within damage range, so damage the closest one.
		{
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
		if (shipHealth >= 0)
		{
			shipHealth -= damageToTake;
			shipHealthText.text = shipHealth.ToString();
			shipHealthBar.fillAmount = shipHealth / shipMaxHP;
		}
		
	}

	private void DamageCannons(GameObject cannonToDamage, int damageToTake)
	{
        if (bulletType == EnemyBulletScript.bulletType.sticky)
        {
            cannonToDamage.GetComponentInChildren<CannonController>().slowDebuffTimer = 10;
        }

		cannonToDamage.GetComponentInChildren<CannonController>().damageCannon(damageToTake);
	}
}