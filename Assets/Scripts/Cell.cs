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
    
    //LeftUp, LeftDown, RightUp, RightDown, Left, Up, Center

    [SerializeField] private Sprite LeftUp;
    [SerializeField] private Sprite LeftDown;
    [SerializeField] private Sprite RightUp;
    [SerializeField] private Sprite RightDown;
    [SerializeField] private Sprite Left;
    [SerializeField] private Sprite Up;
    [SerializeField] private Sprite Center;

    public void buildWall()
    {
        string name = this.gameObject.name;
        string[] split = name.Split('x');
        int x = Int32.Parse(split[0]);
        int y = Int32.Parse(split[1]);
        spriteRenderer.sprite = SpriteChooser(x,y);
        
        GridManager.Instance.nodes[x-1,y].GetCell().UpdateNeighborTexture();
        GridManager.Instance.nodes[x+1,y].GetCell().UpdateNeighborTexture();
        GridManager.Instance.nodes[x,y+1].GetCell().UpdateNeighborTexture();
        GridManager.Instance.nodes[x,y-1].GetCell().UpdateNeighborTexture();
        
    }

    public void UpdateNeighborTexture()
    {
        string name = this.gameObject.name;
        string[] split = name.Split('x');
        int x = Int32.Parse(split[0]);
        int y = Int32.Parse(split[1]);
        spriteRenderer.sprite = SpriteChooser(x,y);
    }

    public Sprite SpriteChooser(int x, int y)
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        
        Node nd = GridManager.Instance.nodes[x, y];
        if (nd.GetUsed())
        {
            if(GridManager.Instance.nodes[x-1,y].GetUsed() && GridManager.Instance.nodes[x,y+1].GetUsed())
                return LeftUp;
            else if(GridManager.Instance.nodes[x-1,y].GetUsed() && GridManager.Instance.nodes[x,y-1].GetUsed())
                return LeftDown;
            else if(GridManager.Instance.nodes[x+1,y].GetUsed() && GridManager.Instance.nodes[x,y+1].GetUsed())
                return RightUp;
            else if (GridManager.Instance.nodes[x + 1, y].GetUsed() && GridManager.Instance.nodes[x, y - 1].GetUsed())
                return RightDown;
            else if (GridManager.Instance.nodes[x - 1, y].GetUsed())
                return Left;
            else if (GridManager.Instance.nodes[x + 1, y].GetUsed())
                return Left;
            else if(GridManager.Instance.nodes[x,y+1].GetUsed())
                return Up;
            else if(GridManager.Instance.nodes[x,y-1].GetUsed())
                return Up;
            else
                return Center;
        }

        return null;
    }

    public void ChangeColor(Color col)
    {
        spriteRenderer.color = col;
    }

    public void RemoveColor()
    {
        spriteRenderer.color = new Color(0,0,0,0);
    }
}

