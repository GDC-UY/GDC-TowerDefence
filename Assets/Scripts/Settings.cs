using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject settingsCanvas;
    public bool STATUS = false;

    public void OpenSettings() //Freno el tiempo para que nada se ejecute en el background
    {
        Time.timeScale = 0;
        Game.CameraShouldMove = false;
        settingsCanvas.SetActive(true);
        STATUS = true;
    }

    public void CloseSettings()
    {
        Time.timeScale = 1; 
        Game.CameraShouldMove = true;
        settingsCanvas.SetActive(false);
        STATUS = false;
    }
}
