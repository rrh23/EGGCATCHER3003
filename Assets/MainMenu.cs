using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

    private void Start()
    {
        GameObject settingsMenu = GameObject.Find("Settings Menu");
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
       Application.Quit();
    }
}
