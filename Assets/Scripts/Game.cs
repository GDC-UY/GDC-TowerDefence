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
    public int gold;

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

        if (isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
            
            Debug.Log("Entra?");
            if (hit.collider.gameObject != null  && (this.gold >= 1000))
            {
                BuildOnCell(hit.collider.gameObject);
                // El jugador pierde 1000 en la construccion.
                this.LoseMoney(1000);
                Debug.Log("Oro restante: "+ this.gold);
            }
        }
        
        // Se toma 3000 como precio placeholder de una torre, si no se tiene la plata no se construye
        if (isTowerBuildModeOn && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
            Cell hittedCell = hit.collider.gameObject.GetComponent<Cell>();
            Debug.Log("Entro?");
            if (hit.collider != null && hittedCell.node.GetUsed() && !hittedCell.HasAttachedTurret()  && (this.gold >= 3000))
            {
                // Se instancia y pone la torreta
                GameObject turret = Instantiate(tower, hit.collider.gameObject.transform.position, Quaternion.identity);
                hittedCell.AttachTurret(turret);
                // El jugador pierde 3000 en la construccion.
                this.LoseMoney(3000);
                Debug.Log("Oro restante:  "+ this.gold);
            }
        }

        // Check for Control + Z or right-click
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1))
        {
            // El jugador recupera 50% de la plata que le costo hacer la torre, por ahora 1500
            this.RecieveMoney(1500);
            DestroyCell();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Enemy, new Vector3(-9.5f, -9.5f, 10 ), Quaternion.identity);
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

    private Stack<Cell> StackCZ = new Stack<Cell>();
    

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

    public void RecieveMoney(int oro) {
        this.gold += oro;
    }

    public void LoseMoney(int oro) {
        if (this.gold - oro < 0)
        {
            this.gold=0;
        }
        else {
            this.gold =this.gold - oro;
        }
    }
}
