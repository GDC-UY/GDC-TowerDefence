using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreaker : Enemy
{
    public int wallBreakPower = 1;
    public float wallBreakTriggerDistance = 5f; //La distancia del muro a la que explota

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Checkea si esta a la distancia necesaria del muro
        //if (Vector3.Distance(transform.position, targetPosition) <= wallBreakTriggerDistance)
        //{
            BreakWall();
        //}
    }

    private void BreakWall()
    {
        // Implement logic to select a random wall to break and break it
        // You might need a separate script to manage walls and their health
    }



    
}
