using System.Collections;
using System.Collections.Generic;
using Scenes;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GridManager gm;
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
        gm.previewPath();
        activateBuildModeButton.onClick.AddListener(EnableBuildMode);
    }

    // Game encarga de los inputs
    void Update()
    {
        if (isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() // el EventSystem realmente no se porque funciona pero es para solucionar que no se ponga un muro cuando desactivas el boton
        )
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);

            if (hit.collider.gameObject != null)
            {
                ChangeCell(hit.collider.gameObject);
            }
        }
    }
    
    public void EnableBuildMode()
    {
        isBuildModeOn = !isBuildModeOn;
    }

    private void ChangeCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        cellToChange.ChangeColor(Color.black);
        gm.updatePath(cellToChange);
        
    }

}
