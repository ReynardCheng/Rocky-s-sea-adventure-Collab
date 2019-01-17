using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [Header("Ship HP")]
    public Image shipHpBarImage;
    public float shipMaxHp = 100f;
    private BoatCombat1 boatCombatScript;

    private BoatController boatControl;


    public Image winScreen;
    public Image loseScreen;

    // Use this for initialization
    void Start()
    {
        boatCombatScript = FindObjectOfType<BoatCombat1>();
        boatControl = FindObjectOfType<BoatController>();
    }
	
	// Update is called once per frame
	void Update () {
        

       // shipHpBarImage.fillAmount = boatCombatScript.shipHealth / shipMaxHp;
       
        if (boatControl.reachedEnd)
        {
            Win();
        }

        if (boatCombatScript.shipHealth <= 0)
        {
            print("Ship died");
            Lose();
        }
     
    }

    void Win()
    {
        winScreen.gameObject.SetActive(true);
        
    }

    void Lose()
    {
        loseScreen.gameObject.SetActive(true);
    }
}
