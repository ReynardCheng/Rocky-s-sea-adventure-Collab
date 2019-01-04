using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [Header("Ship HP")]
    public Image shipHpBarImage;
    public float shipMaxHp = 100f;
    private BoatCombat boatCombatScript;

    public GameObject ThePauseScreen;
    public CharacterMovement PlayerController;

    // Use this for initialization
    void Start()
    {
        boatCombatScript = FindObjectOfType<BoatCombat>();
        PlayerController = FindObjectOfType<CharacterMovement>();
        
    }
	
	// Update is called once per frame
	void Update () {
        

        shipHpBarImage.fillAmount = boatCombatScript.shipHealth / shipMaxHp;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            { //check if game is paused
                ResumeGame(); //when game is paused and press esc button, resume game
            }
            else
            {
                PauseGame(); //when game is running and press esc button, pause game
            }

        }

        
    }

    public void PauseGame()
    {
        Time.timeScale = 0; //freeze the game
        ThePauseScreen.SetActive(true);
        PlayerController.canMove = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f; //resume realtime
        ThePauseScreen.SetActive(false);

    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f; //avoid game freezing when changing screens
        //SceneManager.LoadScene("Main Menu");
    }

}
