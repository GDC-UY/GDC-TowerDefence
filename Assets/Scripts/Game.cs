using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GridManager gm;
    public GameObject cellSelected;
    public Button activateBuildModeButton;
    public Button activateTowerBuildModeButton;
    public Button undoBuildButton;
    public bool isBuildModeOn;
    public bool isTowerBuildModeOn;
    public GameObject Enemy;

    public GameObject tower;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start(){
        undoBuildButton.onClick.AddListener(DestroyCell);
        activateBuildModeButton.onClick.AddListener(EnableBuildMode);
        activateTowerBuildModeButton.onClick.AddListener(EnableTowerBuildMode);
        gm.previewPath();   
    }

    // Game encarga de los inputs
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            ClickHandler();
        }

        // Check for Control + Z or right-click
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1))
        {
            DestroyCell();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Enemy, new Vector3(-9.5f, -9.5f, 10), Quaternion.identity);
        }
    }

    private void ClickHandler()
    {
        // build wall
        if (isBuildModeOn && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);

            if (hit.collider.gameObject != null)
            {
                BuildOnCell(hit.collider.gameObject);
            }
        }
        // fin wall
        // build tower
        if (isTowerBuildModeOn && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
            Cell hittedCell = hit.collider.gameObject.GetComponent<Cell>();

            if (hit.collider != null && hittedCell.node.GetUsed() && !hittedCell.HasAttachedTurret())
            {
                GameObject turret = Instantiate(tower, hit.collider.gameObject.transform.position, Quaternion.identity);
                hittedCell.AttachTurret(turret);
            }
        }
        // fin tower
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
            isTowerBuildModeOn = false;
        }
    }

    public void EnableTowerBuildMode()
    {
        if (isTowerBuildModeOn)
        {
            isTowerBuildModeOn = false;
        }
        else
        {
            isTowerBuildModeOn = true;
            isBuildModeOn = false;
        }
    }
    private void BuildOnCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        cellToChange.ChangeColor(Color.black);
        
        if (!gm.updatePath(cellToChange))
        {
            StackCZ.Push(cellToChange);
        }
        
    }

    private Stack<Cell> StackCZ = new Stack<Cell>();
    

    private void BuildWallOnCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        cellToChange.ChangeTypes(EnumCell.Wall);
        if (!gm.updatePath(cellToChange))
        {
            StackCZ.Push(cellToChange);
        }
        
    }
    
    public void DestroyCell()
    {
        if (StackCZ.Any())
        {
            Cell cellToChange = StackCZ.Pop();
            cellToChange.DeatachTurret();
            cellToChange.node.SetUsed(false);
            cellToChange.ChangeColor(Color.magenta);
            gm.updatePath(cellToChange);
        }
        
    }
}
