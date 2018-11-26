using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHealth : MonoBehaviour {

	public float health;

    private BuildCannon buildCannonScript;

	// Use this for initialization
	void Start () {
        buildCannonScript = GetComponent<BuildCannon>();	
	}
	
    public void SetBeginningHealth(float beginningHealth)
    {
        health = beginningHealth;
    } 

    public void ChangeHealth(float damageToTake, float damageToHeal)
	{
		health -= damageToTake;
        health += damageToHeal;

        if (health <= 0)
        {
            DestroyCannon();
        }
    }

    void DestroyCannon()
    {
        buildCannonScript.slotTaken = false;
        Destroy(buildCannonScript.linkedCannon);
        buildCannonScript.linkedCannon = null;
        buildCannonScript.slotTaken = false;
    }

}
