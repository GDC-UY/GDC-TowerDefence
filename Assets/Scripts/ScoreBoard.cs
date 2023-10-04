using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    private void Start()
    {
        instance = this;
        DEATH_MENU.SetActive(false);
    }
    
    [SerializeField] private GameObject DEATH_MENU;
    
    [SerializeField] private GameObject CONTENT_FATHER;
    [SerializeField] private TMP_InputField INPUT_NAME;
    
    [SerializeField] private TMP_Text WAVE_TEXT;
    
    [SerializeField] private GameObject SCORE_BOARD_ITEM;
    

    public void death()
    {
        checkAndCreateCSV();
        Game.CameraShouldMove = false;
        DEATH_MENU.SetActive(true);
        WAVE_TEXT.text = "Wave " + Game.Instance.roundCounter.ToString();
        loadFromCSV();
        Time.timeScale = 0;
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        Game.CameraShouldMove = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    
    public void PushToScoreBoard()
    {
        INPUT_NAME.interactable = false;
        
        string Text = INPUT_NAME.text;
        if (Text != "")
        {
            string formalText = Text.Substring(0, 1).ToUpper() + Text.Substring(1).ToLower();
            formalText = "Wave " + Game.Instance.roundCounter + " - " + formalText;
            if(SCORE_BOARD_ITEMS != null && SCORE_BOARD_ITEMS.Length > 0)
                foreach (GameObject item in SCORE_BOARD_ITEMS)
                {
                    RectTransform rect = item.GetComponent<RectTransform>();
                    
                    rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y - 22, 1);
                    
                }
            
            GameObject newScore = Instantiate(SCORE_BOARD_ITEM, CONTENT_FATHER.transform);
            newScore.GetComponent<TMP_Text>().text = formalText;
            newScore.GetComponent<TMP_Text>().color = new Color(1, 0.92f, 0.016f, 1);
            saveToCSV(Text, Game.Instance.roundCounter);
        }
    }

    //22 TRANSFORM DIFF//
    private GameObject[] SCORE_BOARD_ITEMS = null;
    int nextTransform = 0;
    
    public void loadFromCSV()
    {
        string path = Application.dataPath + "/scoreboard.csv";
        string[] lines = File.ReadAllLines(path);
        
        int size = lines.Length;
        SCORE_BOARD_ITEMS = new GameObject[size];
        
        int counter = 0;
        Vector3 pos = new Vector3(1.903015f, 138.91f, 1);
        foreach (string line in lines)
        {
            Debug.Log("Working with " + line);
            string[] data = line.Split(',');
            string name = data[0];
            int wave = Int32.Parse(data[1]);
            
            string formalText = name.Substring(0, 1).ToUpper() + name.Substring(1);
            formalText = "Wave " + wave + " - " + formalText;
            
            GameObject newScore = Instantiate(SCORE_BOARD_ITEM, pos, Quaternion.identity, CONTENT_FATHER.transform);
            RectTransform rect = newScore.GetComponent<RectTransform>();
            rect.anchoredPosition = pos;
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 1);
            newScore.GetComponent<TMP_Text>().text = formalText;
            SCORE_BOARD_ITEMS[counter] = newScore;
            counter++;
            pos = new Vector3(pos.x, pos.y - 22, pos.z);
            Debug.Log("Created " + formalText + " at " + newScore.GetComponent<RectTransform>().localPosition);
        }
        
        Debug.Log("Loaded " + counter + " items");
    }

    public void sortCSV()
    {
        string file = Application.dataPath + "/scoreboard.csv";
        string[] lines = File.ReadAllLines(file);
        
        string[] names = new string[lines.Length];
        int[] waves = new int[lines.Length];
        
        int counter = 0;
        foreach (string line in lines)
        {
            string[] data = line.Split(',');
            string name = data[0];
            int wave = Int32.Parse(data[1]);
            
            names[counter] = name;
            waves[counter] = wave;
            counter++;
        }
        
        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves.Length; j++)
            {
                if (waves[i] > waves[j])
                {
                    int temp = waves[i];
                    waves[i] = waves[j];
                    waves[j] = temp;
                    
                    string tempName = names[i];
                    names[i] = names[j];
                    names[j] = tempName;
                }
            }
        }
        
        string textToSave = "";
        for (int i = 0; i < waves.Length; i++)
        {
            textToSave += names[i] + "," + waves[i] + "\n";
        }
        
        File.WriteAllText(file, textToSave);
        Debug.Log("Sorted CSV");
    }

    public void saveToCSV(string Name, int Wave)
    {
        string path = Application.dataPath + "/scoreboard.csv";
        string textToSave = Name + "," + Wave + "\n";
        
        File.AppendAllText(path, textToSave);
        
        Debug.Log("Saved " + Name + " at wave " + Wave + " in " + path);
        Debug.Log("Sorting CSV");
        sortCSV();
    }
    
    public void checkAndCreateCSV()
    {
        if(!File.Exists(Application.dataPath + "/scoreboard.csv"))
        {
            File.Create(Application.dataPath + "/scoreboard.csv");
            
            Debug.Log("Created CSV");
        }
    }
}
