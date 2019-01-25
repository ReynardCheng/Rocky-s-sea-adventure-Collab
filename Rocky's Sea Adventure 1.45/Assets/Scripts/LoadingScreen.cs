using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen theLoadingScreen;

    private void Awake()
    {
       // DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        theLoadingScreen = GetComponent<LoadingScreen>();
	}

    public void loadLevel(string sceneName)
    {

        StartCoroutine(LoadAsynchronously(sceneName));

    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }

    }

}
