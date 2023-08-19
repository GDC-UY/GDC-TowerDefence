using System.Collections;
using System.Collections.Generic;
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
        public void GetUnusedNodes(LinkedList<Node> bfsList){
            Queue<Node> queue = new Queue<Node>();
            visited = true;
            queue.Enqueue(this);
            bfsList.AddLast(this);
            while (queue.Count != 0){
                Node x = queue.Dequeue();
                LinkedList<Node> adyacentes = x.GetAdy();
                foreach (Node adyacente in adyacentes){
                    if (!adyacente.GetVisited() && !adyacente.GetUsed()){
                        adyacente.SetVisited(true);
                        queue.Enqueue(adyacente);
                        bfsList.AddLast(adyacente);
                    }
                }
            }
        }
    }
}