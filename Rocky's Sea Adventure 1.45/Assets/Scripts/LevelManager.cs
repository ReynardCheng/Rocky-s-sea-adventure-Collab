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
	public AudioSource bgm, sfx;
	public AudioClip levelBgmClip, levelClearClip, levelLostClip, bossNearClip, buttonClickClip;
	public AudioClip upgradeMenuClip, boostRefuelClip, collectionClip, upgradingClip, completeUpgradeClip;
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

		if (!endScreen.gameObject.activeSelf){
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
		}

		Collider[] colliders = Physics.OverlapSphere(boatCombatScript.transform.position, changeSkyboxRadius); // Gets all colliders in a radius around the position, and store them into an array.
		bossIsNear = false;

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
					newSoundtrack(levelBgmClip);
					StartCoroutine(ChangeSkyboxColor(SkyboxColor.Default, skyboxTransitionDuration));
				}
			}
			else
			{
				if (currentSkyboxColor == SkyboxColor.Default)
				{
					newSoundtrack(bossNearClip);
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

	public void PlaySoundEffect(AudioClip clip){
		sfx.PlayOneShot(clip);
	}

	public bool IsNearBoss(){
		return currentSkyboxColor == SkyboxColor.Boss;
	}

	public void Win()
	{
		PlaySoundEffect(levelClearClip);
		endScreen.gameObject.SetActive(true);
      // SceneManager.LoadScene(nextLevel);
	}

	void Lose()
	{
		PlaySoundEffect(levelLostClip);
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

	//We create an array with 2 audio sources that we will swap between for transitions
    public static AudioSource[] aud = new AudioSource[2];
    //We will use this boolean to determine which audio source is the current one
    bool activeMusicSource;
    //We will store the transition as a Coroutine so that we have the ability to stop it halfway if necessary
    IEnumerator musicTransition;
 
    void Awake () {
        aud = bgm.GetComponents<AudioSource>();
    }
 
    //use this method to start a new soundtrack, with a reference to the AudioClip that you want to use
    //    such as:        newSoundtrack((AudioClip)Resources.Load("Audio/soundtracks/track01"));
    public void newSoundtrack (AudioClip clip) {
 
        int nextSource = !activeMusicSource ? 0 : 1;
        int currentSource = activeMusicSource ? 0 : 1;
 
        //If the clip is already being played on the current audio source, we will end now and prevent the transition
        if (clip == aud[currentSource].clip)
            return;
 
        //If a transition is already happening, we stop it here to prevent our new Coroutine from competing
        if (musicTransition != null)
            StopCoroutine(musicTransition);
 
        aud[nextSource].clip = clip;
        aud[nextSource].Play();
 
        musicTransition = transition(50); //20 is the equivalent to 2 seconds (More than 3 seconds begins to overlap for a bit too long)
        StartCoroutine(musicTransition);
    }
 
	//  'transitionDuration' is how many tenths of a second it will take, eg, 10 would be equal to 1 second
    IEnumerator transition(int transitionDuration) {
 
        for (int i = 0; i < transitionDuration+1; i++) {
            aud[0].volume = activeMusicSource ? (transitionDuration - i) * (1f / transitionDuration) : (0 + i) * (1f / transitionDuration);
            aud[1].volume = !activeMusicSource ? (transitionDuration - i) * (1f / transitionDuration) : (0 + i) * (1f / transitionDuration);
 
            //  Here I have a global variable to control maximum volume.
            //  options.musicVolume is a float that ranges from 0f - 1.0f
            //------------------------------------------------------------//
            aud[0].volume *= 0.7f;
            aud[1].volume *= 0.7f;
            //------------------------------------------------------------//
 
            yield return new WaitForSecondsRealtime(0.1f);
            //use realtime otherwise if you pause the game you could pause the transition half way
        }
 
        //finish by stopping the audio clip on the now silent audio source
        aud[activeMusicSource ? 0 : 1].Stop();
 
        activeMusicSource = !activeMusicSource;
        musicTransition = null;
    }
}
