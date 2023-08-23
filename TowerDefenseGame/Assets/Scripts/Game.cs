using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GameObject cellSelected;

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Game encarga de los inputs 
    // castea un ray, si este colisiona con una celda, devuelve la celda.
    public Cell getCell(Ray ray){
        if (Physics.Raycast(ray)){
            Debug.Log(Physics.Raycast(ray));
            return null;
        }
        return null;
    }
}
