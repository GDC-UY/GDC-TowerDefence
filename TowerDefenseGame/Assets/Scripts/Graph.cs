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
        public LinkedList<Node> EnemyPathFinding(Node cell){
            ClearVisitedNodes();
            LinkedList<Node> bfsList = new LinkedList<Node>();
            foreach (Node node in nodes){
                if (node.Equals(cell)){
                    node.GetUnusedNodes(bfsList);
                    break;
                }
            }
            return bfsList;
        }
        public LinkedList<Node> EnemyPathFinding(GameObject cell){
            ClearVisitedNodes();
            LinkedList<Node> bfsList = new LinkedList<Node>();
            foreach (Node node in nodes){
                if (node.GetValue().Equals(cell)){
                    node.GetUnusedNodes(bfsList);
                    break;
                }
            }
            return bfsList;
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
    }
}