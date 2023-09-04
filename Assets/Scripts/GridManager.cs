using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public int Width;
    public int Height;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject container;
    public Graph graph;
    public Node[,] nodes;
    private static GridManager instance;
    [SerializeField] private GameObject Enemy;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
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
    // Start is called before the first frame update
    void Start()
    {
        GridPosition();
        nodes = new Node[Width, Height];
        GridCreate();
        CreateGraphConnections();
        Instantiate(Enemy, new Vector3(-2f, -2f, 0), Quaternion.identity);
    }

    private GameObject EnemySpawn;
    private GameObject EnemyTarget;
    private LinkedList<Node> path = null;
    public LinkedList<Node> GetPath()
    {
        //Cache
        if (this.path == null)
            this.path = this.graph.EnemyPathFinding(EnemySpawn, EnemyTarget);
        return this.path;
    }

    private void GridCreate()
    {
        graph = new Graph();
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                GameObject cell = Instantiate(cellPrefab);
                cell.transform.position = new Vector3(
                    row + 0.5f, col + 0.5f, 0); // le sumamos la diferencia del largo de la celda 
                cell.name = $"{row}x{col}";
                //TEMPORAL --------------------------------------------------
                if (cell.name == "0x0")
                {
                    //INICIO
                    cell.GetComponent<Cell>().ChangeTypes(EnumCell.EnemySpawn);
                    EnemySpawn = cell;
                }
                else if (cell.name == "19x19")
                {
                    //FINAL
                    // cell.GetComponent<SpriteRenderer>().color = Color.green;
                    cell.GetComponent<Cell>().ChangeTypes(EnumCell.Finish);
                    EnemyTarget = cell;
                }
                //TEMPORAL --------------------------------------------------
                cell.transform.SetParent(transform);
                Node node = new Node(cell);
                nodes[row, col] = node; // Asignar el objeto a la matriz
                graph.AddNode(node);
                if (cell.name == "15x0" || cell.name == "1x0" || cell.name == "3x5" || cell.name == "3x1" || cell.name == "3x2" || cell.name == "3x3" || cell.name == "19x18" || cell.name == "18x18" || cell.name == "17x18" || cell.name == "17x17" || cell.name == "10x10")
                {
                    // cell.GetComponent<SpriteRenderer>().color = Color.black;
                    // node.SetUsed(true);
                    cell.GetComponent<Cell>().ChangeTypes(EnumCell.Wall);
                }

                cell.GetComponent<Cell>().node = node;
            }
        }
    }

    private void CreateGraphConnections()
    {
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                if (row > 0)
                {
                    graph.AddEdge(nodes[row, col], nodes[row - 1, col]);
                }

                if (row < Width - 1)
                {
                    graph.AddEdge(nodes[row, col], nodes[row + 1, col]);
                }

                if (col < Height - 1)
                {
                    graph.AddEdge(nodes[row, col], nodes[row, col + 1]);
                }

                if (col > 0)
                {
                    graph.AddEdge(nodes[row, col], nodes[row, col - 1]);
                }
            }
        }
    }
    private void PrintGrid()
    {
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                Debug.Log(nodes[row, col].GetValue().name);
            }
        }
    }

    private void PrintEdges()
    {
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                Node[] related = nodes[row, col].GetAdy().ToArray();
                if (related.Length > 2 && related.Length <= 3)
                {
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[2].GetValue().name);
                }
                else if (related.Length > 3)
                {
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[2].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[3].GetValue().name);
                }
                else
                {
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row, col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                }
            }
        }
    }

    private void GridPosition()
    {
        transform.position = new Vector3(0.5f, 0.5f, 0);
    }
    // castea un ray, si este colisiona con una celda, devuelve la celda.
    public GameObject GetCellFromRaycast(RaycastHit2D hit)
    {
        Debug.Log(hit.point.x + " " + hit.point.y);
        int x = Mathf.FloorToInt(hit.point.x);
        int y = Mathf.FloorToInt(hit.point.y);
        if (x >= 0 && x < GridManager.Instance.Width &&
            y >= 0 && y < GridManager.Instance.Height)
        {
            // return Instance.nodes[x, y].GetCell();
            return hit.collider.gameObject;
        }
        return null;
        // transform.position = new Vector3(
            // transform.position.x - (Width / 2) + 0.5f, transform.position.y - (Height / 2) + 0.5f, 0);
    }
}
