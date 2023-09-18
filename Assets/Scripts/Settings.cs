using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject settingsCanvas;

    public void OpenSettings() //Freno el tiempo para que nada se ejecute en el background
    {
        Time.timeScale = 0;
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        Time.timeScale = 1; 
        settingsCanvas.SetActive(false);
    }
}
