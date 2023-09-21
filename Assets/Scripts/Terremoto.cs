using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terremoto : MonoBehaviour
{
    public float interval = 30f; // Intervalo
    public int minimaDestruccion = 3; // Minimas paredes que se deben destruir

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TriggerEarthquake()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            // Seleccionar pared random
            // pared.DestroyWall(); o algo asi
        }
    }

}
