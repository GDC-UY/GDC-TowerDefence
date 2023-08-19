using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridSize;
    [SerializeField] private GameObject cellPrefab;
    public Graph graph;
    public Node[,] nodes;
    // Start is called before the first frame update
    void Start()
    {
        nodes = new Node[gridSize, gridSize];
        GridCreate();
        CreateGraphConnections();
        PrintEdges();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GridCreate()
    {
        graph = new Graph();
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(transform.position.x + row, transform.position.y + col, 0 ), Quaternion.identity);
                cell.name = $"{row}X{col}";
                Node node = new Node(cell);
                nodes[row, col] = node; // Asignar el objeto a la matriz
                graph.AddNode(node);
            }
        }
    }

    private void CreateGraphConnections()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (row > 0)
                {
                    graph.AddEdge(nodes[row, col], nodes[row - 1, col]);
                }

                if (row < gridSize - 1)
                {
                    graph.AddEdge(nodes[row, col], nodes[row + 1, col]);
                }

                if (col < gridSize - 1)
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
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Debug.Log(nodes[row,col].GetValue().name);
            }
        }
    }

    private void PrintEdges()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Node[] related = nodes[row, col].GetAdy().ToArray();
                if (related.Length > 2 && related.Length <= 3)
                {
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[2].GetValue().name);
                }else if (related.Length > 3)
                {
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[1].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[2].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[3].GetValue().name);
                }else
                {
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[0].GetValue().name);
                    Debug.Log(nodes[row,col].GetValue().name + "esta relacionado con " + related[1].GetValue().name);
                }
                
            }
        }
    }

}
