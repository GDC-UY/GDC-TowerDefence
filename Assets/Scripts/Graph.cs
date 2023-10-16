using UnityEngine;
using System.Collections.Generic;

namespace Scenes
{
    public class Graph
    {
        private LinkedList<Node> nodes;

        public Graph()
        {
            nodes = new LinkedList<Node>();
        }

        public void AddNode(Node node)
        {
            if (!nodes.Contains(node))
                nodes.AddLast(node);
        }
    
        public void AddEdge(Node node1, Node node2){
            foreach (Node node in nodes){
                if (node.GetValue().Equals(node1.GetValue())){
                    node.AddEdge(node2);
                }
            }
        }
        public void ClearVisitedNodes(){
            foreach (Node node in nodes){
                node.SetVisited(false);
            }
        }
        public LinkedList<Node> EnemyPathFinding(Node initial, Node destiny){
            ClearVisitedNodes();
            return initial.EnemyPathFinding(destiny);
        }
        public LinkedList<Node> EnemyPathFinding(GameObject initial, GameObject destiny){
            ClearVisitedNodes();
            Node Start = null;
            Node End = null;
            foreach (Node node in nodes)
            {
                if (node.GetValue().Equals(initial))
                {
                    Start = node;
                    
                }else if (node.GetValue().Equals(destiny))
                {
                    End = node;
                }
            }
            if(Start != null && End != null)
                return Start.EnemyPathFinding(End);
            
            return null;
        }
        public LinkedList<Node> GetUnusedNodes()
        {
            LinkedList<Node> unusedNodes = new LinkedList<Node>();
            foreach (Node node in nodes)
            {
                if (!node.GetUsed())
                {
                    unusedNodes.AddLast(node);
                }
            }

            return unusedNodes;
        }
        public LinkedList<Node> GetFreeWalls()
        {
            LinkedList<Node> usedNodes = new LinkedList<Node>();
            foreach (Node node in nodes)
            {
                if (node.GetUsed() && !node.GetCell().HasAttachedTurret())
                {
                    usedNodes.AddLast(node);
                }
            }

            return usedNodes;
        }
        public LinkedList<Node> AStarPathFinding(GameObject initial, GameObject destiny)
        {
            ClearVisitedNodes();
            Node start = null;
            Node end = null;
            foreach (Node node in nodes)
            {
                if (node.GetValue().Equals(initial))
                {
                    start = node;
                }else if (node.GetValue().Equals(destiny))
                {
                    end = node;
                }
            }
            if (start != null && end != null)
            {
                Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
                Dictionary<Node, float> gScore = new Dictionary<Node, float>();
                Dictionary<Node, float> fScore = new Dictionary<Node, float>();
                SortedSet<Node> openSet = new SortedSet<Node>(Comparer<Node>.Create((a, b) =>
                {
                    if (a.Equals(b)) return 0;
                    return fScore[a].CompareTo(fScore[b]);
                }));
                // Inicializar gScore y fScore para todos los nodos a infinito (excepto el nodo inicial)
                foreach (var node in nodes)
                {
                    gScore[node] = float.MaxValue;
                    fScore[node] = float.MaxValue;
                }
                gScore[start] = 0;
                fScore[start] = HeuristicCostEstimate(start, end);

                openSet.Add(start);

                while (openSet.Count > 0)
                {
                    Node current = openSet.Min;
                    openSet.Remove(current);
                    current.SetVisited(true);
                    if (current.Equals(end))
                    {
                        // Reconstruir y devolver el camino si se ha encontrado el destino
                        return ReconstructPath(start, end, cameFrom);
                    }


                    foreach (Node neighbor in current.GetAdy())
                    {
                        if (neighbor.GetUsed() || neighbor.GetVisited())
                        {
                            continue;
                        }
                        float tentativeGScore = gScore[current] + DistanceBetween(current, neighbor);
                        if (tentativeGScore < gScore[neighbor] || !neighbor.GetUsed())
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end);
                            if (!neighbor.GetVisited()) 
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
                
            }
            Debug.LogError("NO HAY CAMINO");
            return null; // No se encontró un camino
        }

        private float HeuristicCostEstimate(Node node, Node end)
        {
            Vector3 pos1 = node.GetValue().transform.position;
            Vector3 pos2 = end.GetValue().transform.position;
            return Vector3.Distance(pos1, pos2);
        }
        private float DistanceBetween(Node node1, Node node2)
        {
            Vector3 pos1 = node1.GetValue().transform.position;
            Vector3 pos2 = node2.GetValue().transform.position;
            return Vector3.Distance(pos1, pos2);
        }
        private LinkedList<Node> ReconstructPath(Node start, Node current, Dictionary<Node, Node> cameFrom)
        {
            LinkedList<Node> path = new LinkedList<Node>();
            while (current != null)
            {
                path.AddFirst(current);
                current = cameFrom.ContainsKey(current) ? cameFrom[current] : null;
            }
            path.AddFirst(start);
            return path;
        }

        
    }
}