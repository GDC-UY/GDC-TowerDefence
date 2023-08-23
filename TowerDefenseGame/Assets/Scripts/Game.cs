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
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            GameObject selected = getCell(TouchRay);
            if( selected != null){
                cellSelected = selected;
            } 
        }
    }
    // Game encarga de los inputs 
    // castea un ray, si este colisiona con una celda, devuelve la celda.
    public GameObject getCell(Ray ray){
        if (Physics.Raycast(ray, out RaycastHit hit)){
            Debug.Log(hit.point.x + " " + hit.point.y);
            int x = (int)hit.point.x;
            int y = (int)hit.point.y;
            if(x >= 0 && x < GridManager.Instance.Width &&
             y >= 0 && y < GridManager.Instance.Height)
            return GridManager.Instance.nodes[x, y].GetCell();
        }
        return null;
    }
}
