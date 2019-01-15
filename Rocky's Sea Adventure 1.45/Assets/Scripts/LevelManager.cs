using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [Header("Ship HP")]
    public Image shipHpBarImage;
    public float shipMaxHp = 100f;
    private BoatCombat boatCombatScript;


    // Use this for initialization
    void Start()
    {
        boatCombatScript = FindObjectOfType<BoatCombat>();
        
    }
	
	// Update is called once per frame
	void Update () {
        

        shipHpBarImage.fillAmount = boatCombatScript.shipHealth / shipMaxHp;
       

    }
}
