using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    public GameObject jugador;

    void Start (){
        jugador = GameObject.Find("Jugador");
    }

    public void OnMouseDown(){
        jugador.GetComponent<Jugador>().grilla_seleccionada = gameObject;
    }

}

