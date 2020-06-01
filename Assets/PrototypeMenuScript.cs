using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeMenuScript : MonoBehaviour
{
    public GameObject InfoMenu;

    public GameObject MainMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartMenu()
    {
        InfoMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void InformationMenu()
    {
        MainMenu.SetActive(false);
        InfoMenu.SetActive(true);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
