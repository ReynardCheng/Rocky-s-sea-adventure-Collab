using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCombat1 : MonoBehaviour
{

	public float shipHealth = 100f;
	public GameObject[] cannonHolder;

	[SerializeField] private LayerMask cannonMask;

	public void TakeDamage(int damageToTake, GameObject damageLocation)
	{
		Collider[] cannonsInRange = Physics.OverlapSphere(damageLocation.transform.position, 5.0f, cannonMask);
		print(cannonsInRange.Length);
		if (cannonsInRange.Length == 0)
		{
			print("heya");
			DamageShip(damageToTake);
		}
		else
		{
			print("dqdddddddqw");
			DamageCannons(cannonsInRange, damageToTake, damageLocation);
		}
	}


	private void DamageShip(float damageToTake)
	{
		shipHealth -= damageToTake;
	}

	private void DamageCannons(Collider[] cannonsInRange, int damageToTake, GameObject damageLocation)
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

		//Finding which Cannon To Damage End
		cannonToDamage.GetComponentInChildren<CannonController>().damageCannon(damageToTake);
	}
}