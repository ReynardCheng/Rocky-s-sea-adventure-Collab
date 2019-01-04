using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonHealth : MonoBehaviour {

    public Image cannonHpBarImage;
    public float cannonMaxHp = 30f;
	public float cannonCurrentHp;

    private BuildCannon buildCannonScript;

	// Use this for initialization
	void Start () {
        buildCannonScript = GetComponent<BuildCannon>();
	}

	private void Update()
	{

	}


	//public void SetBeginningHealth(float beginningHealth)
	//{
	//    cannonCurrentHp = beginningHealth;
	//} 
	public void SetBeginningHealth(float beginningHealth)
    {
        cannonCurrentHp = beginningHealth;
        //cannonHpBarImage = linkedCannon.GetComponentInChildren<Image>();
        
        cannonHpBarImage = buildCannonScript.linkedCannon.GetComponentInChildren<Image>();
    }


    public void ChangeHealth(float damageToTake, float damageToHeal)
	{
        print("Dmg to take:" + damageToTake);
        cannonCurrentHp -= damageToTake;
        cannonCurrentHp += damageToHeal;

        print("cannonCurrentHp:" + cannonCurrentHp / cannonMaxHp);
        cannonHpBarImage.fillAmount = cannonCurrentHp / cannonMaxHp;

		if (cannonCurrentHp <= 0)
		{
			DestroyCannon();
		}
	}

    void DestroyCannon()
    {
        cannonHpBarImage.gameObject.SetActive(false);
        buildCannonScript.slotTaken = false;
        Destroy(buildCannonScript.linkedCannon);
        buildCannonScript.linkedCannon = null;
        buildCannonScript.slotTaken = false;
		Destroy(gameObject);
	}

}
