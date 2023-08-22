using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    public void OnMouseDown(){
        Debug.Log("Click en la celda" + GetComponent<Cell>().transform.name);
        Jugador.Instance.grillaSelecionada = gameObject;
    }
}

