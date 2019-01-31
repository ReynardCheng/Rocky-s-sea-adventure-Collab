using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen theLoadingScreen;
    public GameObject loadScreen;
    public Slider loadingBar;
    public static bool loaded;


    //public AudioSource theAudio;


    private void Awake()
    {
        if (loaded ==true)
        {
            Destroy(gameObject);
        }
      
    }

    // Use this for initialization
    void Start ()
    {
        loaded = true;
        theLoadingScreen = GetComponent<LoadingScreen>();
        //theAudio = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (loadScreen.activeInHierarchy) //check if loading screen gameobject is active
        {
            print("loadScreen Active");
        }
    }

    public void loadLevel(string sceneName)
    {

        StartCoroutine(LoadAsynchronously(sceneName));

    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine
        loadScreen.SetActive(true);

        //theAudio.Play();


        while (!operation.isDone) // while the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            loadingBar.value = progress; //loading bar reflects load progress

            yield return null;
        }
        if (operation.isDone)
        {
            //set the loading screen false on load
            loadScreen.SetActive(false);
        }
    }

}


