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
    private Stack<GameObject> StackCZ = new Stack<GameObject>();
    [SerializeField] private GameObject[] towers;
    private TowerType towerType;
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
            //Para evitar errores de null reference exeption debido a la hitbox de la torreta, el raycast solo detecta objetos de la capa Cell
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction, Mathf.Infinity, LayerMask.GetMask("Cell")); 
            Cell hitCell = hit.collider.gameObject.GetComponent<Cell>();
            if (hit.collider.gameObject != null  && (this.gold >= hitCell.getCost()) && !hitCell.node.GetUsed())
            {
                BuildOnCell(hit.collider.gameObject);
                Debug.Log("Oro restante: "+ this.gold);
            }
        }
        
        if (isTowerBuildModeOn && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            towerType = TowerType.Tower0;
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction, Mathf.Infinity, LayerMask.GetMask("Cell"));
            Cell hitCell = hit.collider.gameObject.GetComponent<Cell>();
            Tower tower = towers[(int)towerType].gameObject.GetComponent<Tower>();
            if (hitCell != null) //Para que no de null reference exeption
            {
                if (hit.collider != null && hitCell.node.GetUsed() && !hitCell.HasAttachedTurret()  && (this.gold >= tower.getCost())) //Esta es la linea de error null reference exeption
                {
                    // Se instancia y pone la torreta
                    GameObject turret = Instantiate(towers[(int)towerType], hit.collider.gameObject.transform.position, Quaternion.identity);
                    hitCell.AttachTurret(turret);
                    StackCZ.Push(turret);
                    LoseMoney(tower.getCost());
                    Debug.Log("Oro restante:  "+ this.gold);
                }    
            }
            
        }

        // Check for Control + Z or right-click
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1))
        {
            DestroyCell();
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


    

    private void BuildOnCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        
        cellToChange.buildWall();
        
        if (!gm.updatePath(cellToChange))
        {
            LoseMoney(cell.GetComponent<Cell>().getCost());
            StackCZ.Push(cell);
            cellToChange.WallBuilded();
        }       
    }
    
    public void DestroyCell()
    {
        if (StackCZ.Any())
        {
            GameObject gamePop = StackCZ.Pop();
            if (gamePop.tag == "Cell") {
                // El jugador recupera 50% de la plata que le costo hacer la torre, por ahora 1500
                Cell cell = gamePop.gameObject.GetComponent<Cell>();
                this.RecieveMoney(cell.getCost() / 2);

                cell.node.SetUsed(false);
                cell.RemoveWall();
                gm.updatePath(cell);
            }
            else {
                Cell cell = gamePop.transform.parent.gameObject.GetComponent<Cell>();
                cell.DeatachTurret();
                this.RecieveMoney(gamePop.GetComponent<Tower>().getCost() / 2);
            }
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
    private enum TowerType
    {
        Tower0,
    }
}
