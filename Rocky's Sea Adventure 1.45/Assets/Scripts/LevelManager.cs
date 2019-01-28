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

    [Header("Misc")]
    public Color skyboxBossColor;
    public Color skyboxDefaultColor;
    public float changeSkyboxRadius;
    public float skyboxTransitionDuration;
    public GameObject boss;
    private SkyboxColor currentSkyboxColor;

    public enum SkyboxColor
    {
        Default, Boss
    }

    public Image winScreen;
    public Image loseScreen;

    // Use this for initialization
    void Start()
    {
        boatCombatScript = FindObjectOfType<BoatCombat1>();
        boatControl = FindObjectOfType<BoatController>();

        RenderSettings.skybox.SetColor("_Tint", skyboxDefaultColor);
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

        Collider[] colliders = Physics.OverlapSphere(boatCombatScript.transform.position, changeSkyboxRadius); // Gets all colliders in a radius around the position, and store them into an array.
        bool bossIsNear = false;

        if (colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                if (c.gameObject == boss)
                {
                    bossIsNear = true;
                    
                    break;
                }
            }

            if (!bossIsNear)
            {
                if (currentSkyboxColor == SkyboxColor.Boss)
                {
                    StartCoroutine(ChangeSkyboxColor(SkyboxColor.Default, skyboxTransitionDuration));
                }
            } else
            {
                if (currentSkyboxColor == SkyboxColor.Default)
                {
                    StartCoroutine(ChangeSkyboxColor(SkyboxColor.Boss, skyboxTransitionDuration));
                }
            }
        }
     
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(boatCombatScript.transform.position, changeSkyboxRadius);
    }

    public IEnumerator ChangeSkyboxColor(SkyboxColor sc, float duration)
    {
        Color oldColor = RenderSettings.skybox.GetColor("_Tint");
        Color newColor = sc == SkyboxColor.Default ? skyboxDefaultColor : skyboxBossColor;
        currentSkyboxColor = sc;
        float maxDuration = duration;

        while (duration > 0f)
        {
            Color c = Color.Lerp(oldColor, newColor, (maxDuration - duration)/maxDuration);
            RenderSettings.skybox.SetColor("_Tint", c);
            //print(duration);

            duration -= Time.deltaTime;

            yield return null;
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
