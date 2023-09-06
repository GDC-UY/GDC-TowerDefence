using System;
using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void ChangeColor(Color col)
    {
        spriteRenderer.color = col;
    }
}

