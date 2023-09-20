using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;
    public GridManager gm;
    public GameObject cellSelected;
    public Button activateBuildModeButton;
    public Button undoBuildButton;
    public bool isBuildModeOn;
    public bool isTowerBuildModeOn;
    public GameObject Enemy;
    public int gold;
    private Stack<GameObject> StackCZ = new Stack<GameObject>();
    public GameObject notEnoughGoldText;
    public TMP_Dropdown dropdown;

    public GameObject tower1, tower2, tower3;
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
        gm.previewPath();

        dropdown.onValueChanged.AddListener(delegate
        {
            EnableTowerBuildMode();
        });
    }

    // Game encarga de los inputs
    void Update()
    {
        if (isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
            
            if (hit.collider.gameObject != null)
            {
                if ((this.gold >= 1000))
                {
                     BuildOnCell(hit.collider.gameObject);
                    // El jugador pierde 1000 en la construccion.
                    this.LoseMoney(1000);
                    Debug.Log("Oro restante: "+ this.gold);
                }
                else
                {
                    StartCoroutine(NotEnoughGoldText());
                }
            }
        }
        
        // Se toma 3000 como precio placeholder de una torre, si no se tiene la plata no se construye
        if (!isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
            Cell hittedCell = hit.collider.gameObject.GetComponent<Cell>();
            if (hit.collider != null && hittedCell.node.GetUsed() && !hittedCell.HasAttachedTurret())
            {
                if ((this.gold >= 3000))
                {
                    GameObject turret = null;
                    switch (dropdown.value)
                    {
                        case 1:
                            turret = Instantiate(tower1, hit.collider.gameObject.transform.position, Quaternion.identity);
                            break;
                        case 2:
                            turret = Instantiate(tower2, hit.collider.gameObject.transform.position, Quaternion.identity);
                            break;
                        case 3:
                            turret = Instantiate(tower3, hit.collider.gameObject.transform.position, Quaternion.identity);
                            break;
                    }
                    // Se instancia y pone la torreta
                    hittedCell.AttachTurret(turret);
                    StackCZ.Push(turret);
                    // El jugador pierde 3000 en la construccion.
                    this.LoseMoney(3000);
                    Debug.Log("Oro restante:  " + this.gold);
                }
                else
                {
                    StartCoroutine(NotEnoughGoldText());
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

        dropdown.value = 0;
    }

    public void EnableTowerBuildMode()
    {
        isBuildModeOn = false;
    }

    private void BuildOnCell(GameObject cell)
    {
        Cell cellToChange = cell.GetComponent<Cell>();
        cellToChange.node.SetUsed(true);
        cellToChange.buildWall();
        
        if (!gm.updatePath(cellToChange))
        {
            StackCZ.Push(cell);
        }       
    }
    
    public void DestroyCell()
    {
        if (StackCZ.Any())
        {
            GameObject gamePop = StackCZ.Pop();
            if (gamePop.tag=="Cell") {
                // El jugador recupera 50% de la plata que le costo hacer la torre, por ahora 1500
                Cell cell = gamePop.gameObject.GetComponent<Cell>();
                this.RecieveMoney(cell.getCost() / 2);

                cell.node.SetUsed(false);
                cell.ChangeColor(Color.magenta);
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

    IEnumerator NotEnoughGoldText()
    {
        notEnoughGoldText.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughGoldText.SetActive(false);
    }
}
