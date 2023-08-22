using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    public GameObject grilla_seleccionada;
    public GameObject objeto_a_colocar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")){
            Instantiate(objeto_a_colocar, grilla_seleccionada.transform);
            
        }   
    }
}