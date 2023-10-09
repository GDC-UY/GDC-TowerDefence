using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static bool CameraShouldMove = true;
    
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
    public int roundCounter;
    private IEnumerator roundTimerCoroutine;
    [SerializeField] private int roundTimer;
    private bool enemiesSpawned;
    public EnemySummoner summoner;
    public TextMeshProUGUI timerMesh, roundMesh, goldText;
    GameObject towerToSpawn;

    [SerializeField] public TMP_Text HealthText;
    public int health = 100;

    
    [SerializeField] private Button SkipWaveButton;

    private enum PossibleGameStates
    {
        Building,
        Defending
    }

    public void UpdateHealth(int cant, Enemy enemy)
    {
        health = health - cant;
        HealthText.text = "Health: " + health;
        enemy.death();
        if (health <= 0)
        {
            HealthText.text = "";
            ScoreBoard.instance.death();
        }
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

void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
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
    
    bool preventDrag = false;
    Vector2 prevPos = Vector2.zero;
    float dragThreshold = 25.0f;

    void Update()
    {
        if (this.gameState.Equals(PossibleGameStates.Building))
        {
            if (Input.GetMouseButtonDown(0))
            {
                preventDrag = false;
                prevPos = Input.mousePosition;
            }

            if (Vector2.Distance(prevPos, Input.mousePosition) > dragThreshold && Input.GetMouseButton(0))
            {
                if (preventDrag)
                {
                    return;
                }
                preventDrag = true;
            }

            if (isBuildModeOn && Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && !preventDrag)
            {
                RaycastHit2D hit = Physics2D.Raycast(TouchRay.origin, TouchRay.direction, Mathf.Infinity, LayerMask.GetMask("Cell"));
                Cell hitCell = hit.collider.gameObject.GetComponent<Cell>();
                if (hit.collider.gameObject != null && (this.gold >= hitCell.getCost()) && !hitCell.node.GetUsed())
                {
                    BuildOnCell(hit.collider.gameObject);
                    this.LoseMoney(hitCell.getCost());
                }
            }
            if (!isBuildModeOn && Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && !preventDrag)
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
                summoner.spawnEnemies(this.enemyPoints);
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
        if(this.gameState == PossibleGameStates.Defending)
            return;
        
        if (dropdown.value > 0)
        {
            dropdown.value = 0;
            dropdown.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        
        if (isBuildModeOn)
        {
            isBuildModeOn = false;
            activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            
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
        activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        
        dropdown.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        
        if (dropdown.value > 0)
        {
            towerToSpawn = towers[dropdown.value - 1]; //el dropdown tiene como valor 0 el cartel que dice "torres"
        }
        else
        {
            dropdown.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
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
            StackCZ.Clear();
            isBuildModeOn = false;
            activateBuildModeButton.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
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
