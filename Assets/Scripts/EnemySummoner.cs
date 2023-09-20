using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySummoner : MonoBehaviour
{
    public GameObject[] enemies;
    public int[] enemiesCost;
    public GameObject cell;
    private int counter;
    public void Start()
    {
        //Sort the enemies by cost, the first one will be the most expensive
        Array.Sort(enemies, delegate (GameObject enemy1, GameObject enemy2)
        {
            return enemy2.GetComponent<Enemy>().cost.CompareTo(enemy1.GetComponent<Enemy>().cost);
        });
        
        //fill enemiesCost in the same slot as enemies
        enemiesCost = new int[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesCost[i] = enemies[i].GetComponent<Enemy>().cost;
        }
    }

    //El codigo deberia de spawnear los enemigos con mas coste primero y luego ir bajando asi rellenar con enemigos de bajo coste hasta ser 0
    //Tambien si sobran puntos deberia de rellenar con enemigos de bajo coste
    public void spawnEnemies(Vector3 enemyBase, int roundPoints)
    {
        int points = roundPoints;
        int index = 0;
        while (points > 0)
        {
            if (points >= enemiesCost[index])
            {
                Instantiate(enemies[index], enemyBase, Quaternion.identity,gameObject.transform);
                points -= enemiesCost[index];
            }
            else
            {
                index++;
            }
        }
    }

    public void enemyDied()
    {
        this.counter--;
    }

    public int getEnemyAmount()
    {
        return this.counter;
    }
    
}
