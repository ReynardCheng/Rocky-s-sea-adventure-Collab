using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneChanger : MonoBehaviour
{


    public void LoadToLevel(string sceneName)
    {
        LoadingScreen.theLoadingScreen.loadLevel(sceneName);
    }

	public void QuitGame()
	{
		Application.Quit();
	}

}