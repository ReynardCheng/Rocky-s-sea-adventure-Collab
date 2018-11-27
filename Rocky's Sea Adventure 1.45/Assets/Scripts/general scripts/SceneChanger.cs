using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{


    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ToLevelSelect()
    {
        SceneManager.LoadScene("Level Selection");
    }

    public void ToAlphaLv1()
    {
        SceneManager.LoadScene("Alpha Lv1(Rocky)");
    }
    public void ToInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void ToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}