using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using Random = System.Random;

public class WallBreaker: MonoBehaviour
{
    public int wallBreakPower = 1;
    public float wallBreakTriggerDistance = 5f; //La distancia del muro a la que explota

    // Start is called before the first frame update
    void Start()
    {
        // Implement logic to select a random wall to break and break it
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            Debug.Log("tecla peruana apretada");
            pelela();
        }
    }

    private void BreakWall()
    {
        // Implement logic to select random walls to break and break it
        //Generate random number between 3 and 5
        GridManager gm = GridManager.Instance;
        int breakRand = UnityEngine.Random.Range(3, 6);
        
        int contador = 0;
        bool check = false;
        Node nodito = null;
        while (contador < breakRand)
        {
            while (check != true)
            {
                int randomX = UnityEngine.Random.Range(0, gm.Width);
                int randomY = UnityEngine.Random.Range(0, gm.Height);
                nodito = gm.nodes[randomX, randomY];
                if (nodito.GetUsed() && !nodito.GetCell().HasAttachedTurret())
                {
                    check = true;
                }
            }
            LinkedList<Node> adyacentes = nodito.GetAdy();
            List<Node> nodosARomper = new List<Node>();
            var adyacente = adyacentes.First;
            while (contador <= breakRand)
            {
                if (adyacente.Value.GetUsed() && !adyacente.Value.GetCell().HasAttachedTurret())
                {
                    adyacente.Value.SetUsed(false);
                    foreach (Node ady in adyacente.Value.GetAdy())
                    {
                        if (ady.GetUsed())
                        {
                            nodosARomper.Add(ady);
                        }
                    }
                    contador++;
                }
                adyacente = adyacente.Next;
                if (adyacente == null)
                {
                    break;
                }
            }
            if(contador < breakRand)
            {
                while(contador <= breakRand)
                {
                    int random = UnityEngine.Random.Range(0, (nodosARomper.Count)-1);
                    if (nodosARomper[random].GetValue() && !nodosARomper[random].GetCell().HasAttachedTurret())
                    {
                        nodosARomper[random].SetUsed(false);
                        contador++;
                    }
                }
            }
        }

        gm.previewPath();

    }

    private void pelela()
    {
        LinkedList<Node> muritos = GridManager.Instance.graph.GetFreeWalls();
        if (muritos.Count > 5)
        {
            Debug.Log("Break Wall");
            BreakWall();
        }
    }


}
