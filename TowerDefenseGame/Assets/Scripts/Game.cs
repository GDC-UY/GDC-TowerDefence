using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GameObject cellSelected;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject selected = GridManager.Instance.getCell(TouchRay);
            if (selected != null)
            {
                cellSelected = selected;
            }
        }
    }
    // Game encarga de los inputs


}
