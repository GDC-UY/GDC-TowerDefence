using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int weight;
    [SerializeField] private int height;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject container;
    public Graph graph;
    public Node[,] nodes;

    [SerializeField] private GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        GridPosition();
        nodes = new Node[weight, height];
        GridCreate();
        CreateGraphConnections();
        
        Instantiate(Enemy, new Vector3(-10.5f, 4.5f, 0 ), Quaternion.identity);
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
        for (int row = 0; row < weight; row++)
        {
            for (int col = 0; col < height; col++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(transform.position.x + row, transform.position.y + col, 0 ), Quaternion.identity);
                cell.name = $"{row}x{col}";
                
                //TEMPORAL --------------------------------------------------
                if (cell.name == "0x0")
                {
                    //INICIO
                    cell.GetComponent<SpriteRenderer>().color = Color.red;
                    EnemySpawn = cell;
                }
                else if (cell.name == "19x19")
                {
                    //FINAL
                    cell.GetComponent<SpriteRenderer>().color = Color.green;
                    EnemyTarget = cell;
                } 
                //TEMPORAL --------------------------------------------------
                    
                cell.transform.SetParent(container.transform);
                Node node = new Node(cell);
                nodes[row, col] = node; // Asignar el objeto a la matriz
                graph.AddNode(node);
                if (cell.name == "15x0")
                {
                    cell.GetComponent<SpriteRenderer>().color = Color.black;
                    node.SetUsed(true);
                }else if (cell.name == "19x16")
                {
                    cell.GetComponent<SpriteRenderer>().color = Color.black;
                    node.SetUsed(true);
                }else if (cell.name == "18x16")
                {
                    cell.GetComponent<SpriteRenderer>().color = Color.black;
                    node.SetUsed(true);
                }

                cell.GetComponent<Cell>().node = node;
            }
        }
    }

    private void CreateGraphConnections()
    {
        for (int row = 0; row < weight; row++)
        {
            for (int col = 0; col < height; col++)
            {
                if (row > 0)
                {
                    graph.AddEdge(nodes[row, col], nodes[row - 1, col]);
                }

                if (row < weight - 1)
                {
                    graph.AddEdge(nodes[row, col], nodes[row + 1, col]);
                }

                if (col < height - 1)
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
        for (int row = 0; row < weight; row++)
        {
            for (int col = 0; col < height; col++)
            {
                Debug.Log(nodes[row,col].GetValue().name);
            }
        }
    }

    private void PrintEdges()
    {
        for (int row = 0; row < weight; row++)
        {
            for (int col = 0; col < height; col++)
            {
                Node[] related = nodes[row, col].GetAdy().ToArray();
                if (related.Length > 2 && related.Length <= 3)
                {
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[2].GetValue().name);
                }else if (related.Length > 3)
                {
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[2].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[3].GetValue().name);
                }else
                {
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + " esta relacionado con " + related[1].GetValue().name);
                }
                
            }
        }
    }

    private void GridPosition()
    {
        transform.position = new Vector3(transform.position.x - (weight/2) + 0.5f, transform.position.y - (height/2) + 0.5f, 0);
    }
}
