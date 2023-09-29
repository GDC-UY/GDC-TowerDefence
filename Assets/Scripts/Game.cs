using System;
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
    public Button endBuildingPhaseButton;
    public bool isBuildModeOn;
    public GameObject Enemy;
    public int gold;
    private Stack<GameObject> StackCZ = new Stack<GameObject>();
    public GameObject notEnoughGoldText;
    public TMP_Dropdown dropdown;
    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    public GameObject[] towers;
    private int enemyPoints;
    private int roundCounter;
    private IEnumerator roundTimerCoroutine;
    [SerializeField] private int roundTimer;
    private bool enemiesSpawned;
    public EnemySummoner summoner;
    public TextMeshProUGUI timerMesh, roundMesh, goldText;
    GameObject towerToSpawn;
    
    [SerializeField] private Button SkipWaveButton;

    private enum PossibleGameStates
    {
        Building,
        Defending
    }

    private PossibleGameStates gameState;

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
        
        gm.previewPath();

        dropdown.onValueChanged.AddListener(delegate
        {
            EnableTowerBuildMode();
        });
        endBuildingPhaseButton.onClick.AddListener(beginDefensePhase);
        gm.previewPath();
        this.enemyPoints = 20;
        this.roundCounter = 1;
        this.roundMesh.SetText("Wave " + roundCounter.ToString());
        this.gameState = PossibleGameStates.Building;
        roundTimerCoroutine = reduceTime();
        StartCoroutine(roundTimerCoroutine);
        enemiesSpawned = false;
        this.SkipWaveButton.interactable = true;
    }
    // Game encarga de los inputs
    void Update()
    {
        if (this.gameState.Equals(PossibleGameStates.Building))
        {
            if (isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //Para evitar errores de null reference exeption debido a la hitbox de la torreta, el raycast solo detecta objetos de la capa Cell
                RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction, Mathf.Infinity, LayerMask.GetMask("Cell"));
                Cell hitCell = hit.collider.gameObject.GetComponent<Cell>();
                if (hit.collider.gameObject != null && (this.gold >= hitCell.getCost()) && !hitCell.node.GetUsed())
                {
                    BuildOnCell(hit.collider.gameObject);
                    this.LoseMoney(hitCell.getCost());
                }
            }
            if (!isBuildModeOn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction, Mathf.Infinity, LayerMask.GetMask("Cell"));
                Cell hitCell = hit.collider.gameObject.GetComponent<Cell>();
                //Tower tower = towers[(int)towerType].gameObject.GetComponent<Tower>();
                if (hitCell != null) //Para que no de null reference exeption
                {
                    if (hit.collider != null && hitCell.node.GetUsed() && !hitCell.HasAttachedTurret()) //&& (this.gold >= tower.getCost())) //Esta es la linea de error null reference exeption
                    {
                        if ((this.gold >= 3000))
                        {
                            GameObject turret = Instantiate(towerToSpawn, hit.collider.gameObject.transform.position, Quaternion.identity);
                            // Se instancia y pone la torreta
                            hitCell.AttachTurret(turret);
                            StackCZ.Push(turret);
                            // El jugador pierde 3000 en la construccion.
                            this.LoseMoney(3000);
                        }
                        else
                        {
                            StartCoroutine(NotEnoughGoldText());
                        }
                    }
                }
            }
            // Check for Control + Z or right-click
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1))
            {
                DestroyCell();
            }
        }
        else
        {
            if (!enemiesSpawned)
            {
                enemiesSpawned = true;
                summoner.spawnEnemies(summoner.gameObject.transform.position, this.enemyPoints);
            }
            else if (summoner.getEnemyAmount() == 0 && enemiesSpawned)
            {
                enemiesSpawned = false;
                this.roundCounter++;
                this.roundTimer = 90;
                this.roundMesh.SetText("Wave " + roundCounter.ToString());
                this.increaseEnemyPoints();
                this.gameState = PossibleGameStates.Building;
                StartCoroutine(reduceTime());
                this.SkipWaveButton.interactable = true;
            }
        }
    }

    private void LateUpdate()
    {
        goldText.text = this.gold.ToString();
    }

    public void EnableBuildMode()
    {
        if (dropdown.value > 0)
        {
            dropdown.value = 0;
            dropdown.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        
        if (isBuildModeOn)
        {
            isBuildModeOn = false;
            activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            
        }
        else
        {
            isBuildModeOn = true;
            activateBuildModeButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            
        }
    }

    public void EnableTowerBuildMode()
    {
        isBuildModeOn = false;
        activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        
        dropdown.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        
        if (dropdown.value > 0)
        {
            towerToSpawn = towers[dropdown.value - 1]; //el dropdown tiene como valor 0 el cartel que dice "torres"
        }
        else
        {
            dropdown.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            towerToSpawn = null;
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
        if (this.gameState == PossibleGameStates.Building)
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
                    cell.RemoveWall();
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

    IEnumerator NotEnoughGoldText()
    {
        notEnoughGoldText.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughGoldText.SetActive(false);
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
        if (this.gameState == PossibleGameStates.Building)
        {
            this.roundTimer = 0;
            this.timerMesh.SetText("Wave in : NOW!");
            this.SkipWaveButton.interactable = false;
            isBuildModeOn = false;
            activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public IEnumerator reduceTime()
    {
        while (this.roundTimer > 0)
        {
            this.timerMesh.SetText("Wave in : " + roundTimer.ToString() + "s");
            yield return new WaitForSeconds(1);
            this.roundTimer--;
        }
        this.gameState = PossibleGameStates.Defending;
        yield return null;
    }
}
