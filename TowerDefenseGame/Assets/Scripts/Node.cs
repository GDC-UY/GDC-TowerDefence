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
        private bool visited;
        private bool used;
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
        public LinkedList<Node> EnemyPathFinding(GameObject initial, GameObject destiny){
            Queue<Node> queue = new Queue<Node>();
            LinkedList<Node> bfsList = new LinkedList<Node>();
            visited = true;
            queue.Enqueue(this);
            bfsList.AddFirst(this);
            while (queue.Count != 0){
                Node x = queue.Dequeue();
                LinkedList<Node> edges = x.GetAdy();
                foreach (Node edge in edges){
                    if (!edge.GetVisited() && !edge.GetUsed()){
                        edge.SetVisited(true);
                        queue.Enqueue(edge);
                        bfsList.AddFirst(edge);
                        if (edge.Equals(destiny))
                        {
                            break;
                        }
                    }
                }
            }
            LinkedList<Node> shortPath = new LinkedList<Node>();
            shortPath.AddFirst(bfsList.First);
            Node temporal = shortPath.First.Value;
            while (!shortPath.First.Value.Equals(bfsList.Last.Value))
            {
                foreach (Node ady in temporal.GetAdy())
                {
                    if (ady.GetVisited())
                    {
                        shortPath.AddFirst(ady);
                        temporal = ady;
                        break;
                    }
                }
            }
            return shortPath;
        }
    }
}