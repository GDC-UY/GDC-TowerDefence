using System;
using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameObject attachedTurret;

    // Precios:
    [SerializeField] private int cost;

    public int getCost() {
        return cost;
    }

    public void AttachTurret(GameObject turret)
    {
        if (attachedTurret == null)
        {
            attachedTurret = turret;
            attachedTurret.transform.SetParent(this.gameObject.transform);
        }
    }

    public bool HasAttachedTurret()
    {
        return attachedTurret != null;
    }

    public void DeatachTurret()
    {
        if (attachedTurret != null)
        {
            Destroy(attachedTurret.gameObject);
            attachedTurret = null;
        }
    }

    public void ChangeColor(Color col)
    {
        spriteRenderer.color = col;
    }
}

