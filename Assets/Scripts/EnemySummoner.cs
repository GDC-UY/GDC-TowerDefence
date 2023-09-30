using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySummoner : MonoBehaviour
{
    public GameObject[] enemies;
    private int[] enemiesCost;
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
    public void spawnEnemies(int roundPoints)
    {
        int index = 0;
        if (enemiesCost.Length != 0 || enemies.Length != 0)
        {
            while (roundPoints > 0)
            {
                Vector2 v = new Vector2(Random.Range(-20.5f, -30.5f), 0.5f);
                
                if (roundPoints >= enemiesCost[index])
                {
                    Instantiate(enemies[index], v, Quaternion.identity, gameObject.transform);
                    roundPoints -= enemiesCost[index];
                    counter++;
                }
                else
                {
                    index++;
                }
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
