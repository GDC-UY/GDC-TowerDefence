using System.Collections;
using System.Collections.Generic;
using Scenes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GameObject cellSelected;
    public Button activateBuildModeButton;
    public bool isBuildModeOn;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    public static Game Instance

    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
                //DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start(){
        activateBuildModeButton.onClick.AddListener(EnableBuildMode);
    }

    // Game encarga de los inputs
    void Update()
    {
        if (isBuildModeOn && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");

            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);

            if (hit.collider != null)
            {
                GameObject selected = hit.collider.gameObject;
                ChangeCell(hit.collider.gameObject);
            }
        }
    }
    
    public void EnableBuildMode()
    {
        if (isBuildModeOn)
        {
            isBuildModeOn = false;
        }
        else
        {
            isBuildModeOn = true;
        }
    }

    private void ChangeCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        cellToChange.GetComponent<SpriteRenderer>().color = Color.black;
        GridManager.updatePath();
    }

}
