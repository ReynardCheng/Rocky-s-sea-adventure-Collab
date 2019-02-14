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
        
		Collider[] cannonsInRange = Physics.OverlapSphere(damageLocation.transform.position, 2, cannonMask);
		print("Cannons in Range:" + cannonsInRange.Length);
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



    //This overload is for boss's beam
    public void TakeDamage(int damageToTake, Vector3 damageLocation)
    {

        Collider[] cannonsInRange = Physics.OverlapSphere(damageLocation, 2, cannonMask);
        for (int i = 0; i < cannonHolder.Length; i++)
        {
            if (cannonHolder[i].GetComponentInChildren<CannonController>() != null)
            {
                if (cannonHolder[i].GetComponentInChildren<CannonController>().cannonType == cannonTypes.defence)
                {
                    if ((cannonHolder[i].transform.position - damageLocation).magnitude < protectionRadius)
                    {
                        DamageCannons(cannonHolder[i].gameObject, damageToTake);
                        return;
                    }
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
                float curDistance = (cannon.gameObject.transform.position - damageLocation).sqrMagnitude;
                if (curDistance < distance)
                    cannonToDamage = cannon.gameObject;
            }

            DamageCannons(cannonToDamage, damageToTake);
        }

    }
}