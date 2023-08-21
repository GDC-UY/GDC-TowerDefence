using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Scenes
{
    public class Node
    {
        private GameObject cell;
        private LinkedList<Node> ady;
        [SerializeField] private bool visited;
        [SerializeField] private bool used;
        public Node(GameObject data){
            cell = data;
            ady = new LinkedList<Node>();
            visited = false;
            used = false;
        }
        public GameObject GetValue(){
            return cell;
        }
        public LinkedList<Node> GetAdy(){
            return ady;
        }

        public bool GetUsed()
        {
            return used;
        }

        public void SetUsed(bool boolean)
        {
            used = boolean;
        }
        public bool GetVisited(){
            return visited;
        }
        public void SetVisited(bool boolean){
            visited = boolean;
        }
        public void AddEdge(Node node)
        {
            if (!ady.Contains(node))
            {
                ady.AddLast(node);
            }
        }

        public LinkedList<Node> EnemyPathFinding(Node destiny)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visitedNodes = new HashSet<Node>();
            Dictionary<Node, Node> parents = new Dictionary<Node, Node>(); // Almacena los padres

            visited = true;
            queue.Enqueue(this);
            visitedNodes.Add(this);
            parents[this] = null; // El nodo inicial no tiene padre

            while (queue.Count != 0) {
                Node x = queue.Dequeue();
                LinkedList<Node> edges = x.GetAdy();

                foreach (Node edge in edges) {
                    if (!visitedNodes.Contains(edge) && !edge.GetUsed()) {
                        edge.SetVisited(true);
                        queue.Enqueue(edge);
                        visitedNodes.Add(edge);
                        parents[edge] = x; // Establece el padre para reconstruir el camino

                        if (edge.Equals(destiny)) {
                            // Encontramos el destino, reconstruir el camino
                            return ReconstructPath(this, destiny, parents);
                        }
                    }
                }
            }
            Debug.LogError("NO HAY CAMINO!");
            return null; // No se encontró un camino
        }

        private LinkedList<Node> ReconstructPath(Node initial, Node destiny, Dictionary<Node, Node> parents) {
            LinkedList<Node> path = new LinkedList<Node>();
            Node current = destiny;

            while (!current.Equals(initial)) {
                path.AddFirst(current);
                current = parents[current];
            }

            path.AddFirst(initial);
            return path;
        }
    }
}