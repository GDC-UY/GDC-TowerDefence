using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
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
    public Button endBuildingPhaseButton;
    public bool isBuildModeOn;
    public bool isTowerBuildModeOn;
    public GameObject Enemy;
    public int gold;
    private Stack<GameObject> StackCZ = new Stack<GameObject>();
    private int enemyPoints;
    private int roundCounter;
    private IEnumerator roundTimerCoroutine;
    [SerializeField] private float roundTimer;
    private bool enemiesSpawned;
    private EnemySummoner summoner;

    private enum PossibleGameStates
    {
        Building,
        Defending
    }

    private PossibleGameStates gameState;
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

    void Start()
    {
        undoBuildButton.onClick.AddListener(DestroyCell);
        activateBuildModeButton.onClick.AddListener(EnableBuildMode);
        activateTowerBuildModeButton.onClick.AddListener(EnableTowerBuildMode);
        endBuildingPhaseButton.onClick.AddListener(beginDefensePhase);
        gm.previewPath();
        this.enemyPoints = 20;
        this.roundCounter = 1;
        this.gameState = PossibleGameStates.Building;
        roundTimerCoroutine = reduceTime(1);
        StartCoroutine(roundTimerCoroutine);
        enemiesSpawned = false;
        summoner = GridManager.Instance.getEnemySummoner();
    }

    // Game encarga de los inputs
    void Update()
    {
        if (this.gameState.Equals(PossibleGameStates.Building))
        {
            if (isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);

                if (hit.collider.gameObject != null && (this.gold >= 1000))
                {
                    BuildOnCell(hit.collider.gameObject);
                    // El jugador pierde 1000 en la construccion.
                    this.LoseMoney(1000);
                    Debug.Log("Oro restante: " + this.gold);
                }
            }

            // Se toma 3000 como precio placeholder de una torre, si no se tiene la plata no se construye
            if (isTowerBuildModeOn && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction);
                Cell hittedCell = hit.collider.gameObject.GetComponent<Cell>();
                if (hit.collider != null && hittedCell.node.GetUsed() && !hittedCell.HasAttachedTurret() &&
                    (this.gold >= 3000))
                {
                    // Se instancia y pone la torreta
                    GameObject turret = Instantiate(tower, hit.collider.gameObject.transform.position,
                        Quaternion.identity);
                    hittedCell.AttachTurret(turret);
                    StackCZ.Push(turret);
                    // El jugador pierde 3000 en la construccion.
                    this.LoseMoney(3000);
                    Debug.Log("Oro restante:  " + this.gold);
                }
            }

            // Check for Control + Z or right-click
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                Input.GetKeyDown(KeyCode.Z) ||
                Input.GetMouseButtonDown(1))
            {
                DestroyCell();
            }
        }
        else
        {
            if (!enemiesSpawned)
            {
                summoner.spawnEnemies(GridManager.Instance.getEnemySummoner().transform.position, enemyPoints);
                enemiesSpawned = true;
            }

            if (summoner.getEnemyAmount() == 0)
            {
                enemiesSpawned = false;
                this.gameState = PossibleGameStates.Defending;
                StartCoroutine(roundTimerCoroutine);
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
            StackCZ.Push(cell);
        }
    }

    public void DestroyCell()
    {
        if (StackCZ.Any())
        {
            GameObject gamePop = StackCZ.Pop();
            if (gamePop.tag == "Cell")
            {
                // El jugador recupera 50% de la plata que le costo hacer la torre, por ahora 1500
                Cell cell = gamePop.gameObject.GetComponent<Cell>();
                this.RecieveMoney(cell.getCost() / 2);

                cell.node.SetUsed(false);
                cell.ChangeColor(Color.magenta);
                gm.updatePath(cell);
            }
            else
            {
                Cell cell = gamePop.transform.parent.gameObject.GetComponent<Cell>();
                cell.DeatachTurret();
                this.RecieveMoney(gamePop.GetComponent<Tower>().getCost() / 2);
            }
        }
    }


    public void RecieveMoney(int oro)
    {
        this.gold += oro;
    }

    public void LoseMoney(int oro)
    {
        if (this.gold - oro < 0)
        {
            this.gold = 0;
        }
        else
        {
            this.gold = this.gold - oro;
        }
    }

    private void increaseEnemyPoints()
    {
        this.enemyPoints += this.enemyPoints / 2;
        if (this.enemyPoints % 2 != 0)
        {
            this.enemyPoints++;
        }
    }

    public void beginDefensePhase()
    {
        this.roundTimer = 0;
    }

    public IEnumerator reduceTime(float waitTime)
    {
        if (this.roundTimer == 0)
        {
            this.gameState = PossibleGameStates.Defending;
            StopCoroutine(roundTimerCoroutine);
        }
        else
        {
            this.roundTimer -= waitTime;
            yield return new WaitForSeconds(waitTime);
        }
    }
}