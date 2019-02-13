using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

	[Header("Ship HP")]
	public Image shipHpBarImage;
	public float shipMaxHp = 100f;
	[SerializeField] BoatCombat1 boatCombatScript;

	private BoatController boatControl;

	[Header("Misc")]
	public Color skyboxBossColor;
	public Color skyboxDefaultColor;
	public float changeSkyboxRadius;
	public float skyboxTransitionDuration;
	public GameObject boss;
	private SkyboxColor currentSkyboxColor;
	bool bossIsNear;
	[SerializeField] myGUI theGui;

	[Header("PauseScreen")]
	public GameObject pauseScreen;
	public bool gamePaused;
	public GameObject miniMap;
	[SerializeField] bool mapActive;

	// change button Text
	public Text mapStatusText;

	// reference to iself
	public static LevelManager theLevelManager;

	public string nextLevel;

	public enum SkyboxColor
	{
		Default, Boss
	}

	public Image endScreen;
	public Image loseScreen;

	// Use this for initialization
	void Start()
	{
		theLevelManager = GetComponent<LevelManager>();
		boatCombatScript = FindObjectOfType<BoatCombat1>();
		boatControl = FindObjectOfType<BoatController>();

		theGui = FindObjectOfType<myGUI>();

		RenderSettings.skybox.SetColor("_Tint", skyboxDefaultColor);

		DynamicGI.UpdateEnvironment();

		mapActive = true;

		miniMap = GameObject.Find("MiniMap");

		//gamePaused = false;
		if (!gamePaused) Time.timeScale = 1;

	}

	// Update is called once per frame
	void Update()
	{

		// shipHpBarImage.fillAmount = boatCombatScript.shipHealth / shipMaxHp;

		if (boatControl.reachedEnd)
		{
			Win();
		}

		if (boatCombatScript.shipHealth <= 0)
		{
			print("Ship died");
			theGui.lose = true;
			Lose();
		}

		Collider[] colliders = Physics.OverlapSphere(boatCombatScript.transform.position, changeSkyboxRadius); // Gets all colliders in a radius around the position, and store them into an array.
		//bossIsNear = false;

		if (colliders.Length > 0)
		{
			foreach (Collider c in colliders)
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
			}
			else
			{
				if (currentSkyboxColor == SkyboxColor.Default)
				{
					StartCoroutine(ChangeSkyboxColor(SkyboxColor.Boss, skyboxTransitionDuration));
				}
			}
		}

		Pause();
		MapStatus();
	}

	private void OnDrawGizmos()
	{
        if (boatCombatScript)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(boatCombatScript.transform.position, changeSkyboxRadius);
        }
	}

	public IEnumerator ChangeSkyboxColor(SkyboxColor sc, float duration)
	{
		Color oldColor = RenderSettings.skybox.GetColor("_Tint");
		Color newColor = sc == SkyboxColor.Default ? skyboxDefaultColor : skyboxBossColor;
		currentSkyboxColor = sc;
		float maxDuration = duration;

		while (duration > 0f)
		{
			Color c = Color.Lerp(oldColor, newColor, (maxDuration - duration) / maxDuration);
			RenderSettings.skybox.SetColor("_Tint", c);
			//print(duration);

			duration -= Time.deltaTime;

			DynamicGI.UpdateEnvironment();

			yield return null;
		}
	}

	public void Win()
	{
		endScreen.gameObject.SetActive(true);
      // SceneManager.LoadScene(nextLevel);
	}

	void Lose()
	{
		endScreen.gameObject.SetActive(true);
	}

	// ----------------------
	// pause related fuctions

	 void Pause()
	{
		if (!gamePaused)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Time.timeScale = 0;
				gamePaused = true;
				pauseScreen.SetActive(true);
				Cursor.visible = true;
			}
		}

		else if (gamePaused)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Time.timeScale = 1;
				gamePaused = false;
				pauseScreen.SetActive(false);
				Cursor.visible = false;
			}
		}
	}

	public void Resume()
	{
		gamePaused = false;
		pauseScreen.SetActive(false);
		Cursor.visible = false;
		Time.timeScale = 1;
	}

	public void retry()
	{
		Time.timeScale = 1;
		LoadingScreen.theLoadingScreen.loadLevel(SceneManager.GetActiveScene().name);
	}

	public void LoadScene(string sceneName)
	{
		LoadingScreen.theLoadingScreen.loadLevel(sceneName);
	}

	void MapStatus()
	{
		if (PlayerPrefs.GetInt("MapActive") == 1)
		{
			mapActive = true;
			miniMap.SetActive(true);
			mapStatusText.text = "mapOpened";
		}

		if (PlayerPrefs.GetInt("MapActive") == 0)
		{
			mapActive = false;
			miniMap.SetActive(false);
			mapStatusText.text = "mapClosed";
		}
	}

	public void ChangeMapStatus()
	{
		if (mapActive)
		{
			PlayerPrefs.SetInt("MapActive", 0);
		}
		else if (!mapActive)
		{
			PlayerPrefs.SetInt("MapActive", 1);
		}
	}
}
