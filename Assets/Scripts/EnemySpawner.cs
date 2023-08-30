using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*
     1) El spawner debe obtener la celda "spawnpoint".
     2) EnemySpawner está encargado de otorgarle al enemigo su path.
     3) A futuro, hay una cantidad prediseñada de rondas, y una vez pasada la ultima ronda diseñada,
     se aplica un multiplicador a la cantidad de enemigos de incremento constante.
     4) Será instanciado múltiples veces y almacenado en listas.
    */
    public LinkedList<Node> enemyPath { get; set; }
    public GameObject spawnPoint { get; set; }
    public GameObject enemyTarget { get; set; }
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies(Dictionary<string, string> enemiesToSpawn, int delay)
    {

    }
}
