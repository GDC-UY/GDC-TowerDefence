using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terremoto : MonoBehaviour
{
    private int minimaDestruccion = 3; // Minimas paredes que se deben destruir
    private int destruccion;
    private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GridManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TriggerEarthquake()    // Falta ver como llamarlo
    {
        while (destruccion < minimaDestruccion)
        {
            destruccion = gridManager.SetRandomNodeAndNeighborsToFalse();
        }
        destruccion = 0;
    }
}
