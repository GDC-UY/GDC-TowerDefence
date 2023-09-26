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

    [SerializeField] private int cost;
    
    public bool cellIsPath = false;
    
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

    [SerializeField] private Sprite TLeft;
    [SerializeField] private Sprite TRight;
    
    [SerializeField] private Sprite TUp;
    [SerializeField] private Sprite TDown;

    [SerializeField] private Sprite CenterNoConectionsOfTheWallForTheEnemiesToNotPassRightThroughBecauseThatWouldBeBadAndWeDontWantThatSoWeHaveToMakeSureThatTheEnemiesDontPassThroughTheWallsBecauseThatWouldBeBadAndWeDontWantThatSoWeHaveToMakeSureThatTheEnemiesDontPassThroughTheWalls;

    [SerializeField] private Sprite EnemyPathSprite;

    private int[] getCoords()
    {
        string name = this.gameObject.name;
        string[] split = name.Split('x');
        int x = Int32.Parse(split[0]);
        int y = Int32.Parse(split[1]);
        
        int[] coords = {x,y};
        return coords;
    }
    
    public void buildWall()
    {
        int[] coords = getCoords();   
        spriteRenderer.sprite = SpriteChooser(coords[0],coords[1]);
    }

    //ESTE METODO SE EJECUTA CUANDO SE CONSTRUYE UNA PARED Y NO INTERUMPE EL PATHFINDING
    //SE LLAMA EN GAME BUILDONCELL
    public void WallBuilded()
    {
        int[] coords = getCoords();
        textureCaller(coords[0],coords[1]);
    }

    public void UpdateNeighborTexture(int x, int y)
    {
        //EN TEORIA SI ESTE METODO SE EJECUTA ESTOY PARADO EN UN NODO QUE TIENE UNA CELDA QUE TIENE UNA PARED Y NO ES NULL
        //PORQUE CARAJOS DAS ERROR AAAAAAAAAAAAAAAAAAAAAAAYUUUUUUUUUUDDDDDDDDDAAAAAAAAAAAAAAA
        spriteRenderer.sprite = SpriteChooser(x,y);
    }
    
    public void textureCaller(int x, int y)
    {
        Cell temp = null;
        
        try{temp = GridManager.Instance.nodes[x, y + 1].GetCell();}catch (Exception e){temp = null;}
        if (temp != null)
        {
            temp.UpdateNeighborTexture(x, y + 1);
        }
        
        try{temp = GridManager.Instance.nodes[x, y - 1].GetCell();}catch (Exception e){temp = null;}
        if (temp != null)
        {
            temp.UpdateNeighborTexture(x, y - 1);
        }
        
        try{temp = GridManager.Instance.nodes[x - 1, y].GetCell();}catch (Exception e){temp = null;}
        if (temp != null)
        {
            temp.UpdateNeighborTexture(x - 1, y);
        }
        
        try{temp = GridManager.Instance.nodes[x + 1, y].GetCell();}catch (Exception e){temp = null;}
        if (temp != null)
        {
            temp.UpdateNeighborTexture(x + 1, y);
        }
    }

    public Sprite SpriteChooser(int x, int y)
    {
        bool arriba = false;
        bool abajo = false;
        bool izquierda = false;
        bool derecha = false;
        
        Node nodoActual = GridManager.Instance.nodes[x, y];
        
        try{arriba = GridManager.Instance.nodes[x, y + 1].GetUsed();}catch (Exception e){arriba = false;}
        try{abajo = GridManager.Instance.nodes[x, y - 1].GetUsed();}catch (Exception e){abajo = false;}
        try{izquierda = GridManager.Instance.nodes[x - 1, y].GetUsed();}catch (Exception e){izquierda = false;}
        try{derecha = GridManager.Instance.nodes[x + 1, y].GetUsed();}catch (Exception e){derecha = false;}
        
        if (nodoActual != null && nodoActual.GetUsed())
        { 
            if (arriba && abajo && izquierda && derecha)
                return Center;
            
            else if (arriba && derecha && izquierda)
                return TUp;
            
            else if (izquierda && derecha && abajo)
                return TDown;
            
            else if (izquierda && arriba && abajo)
                return TLeft;
            
            else if (arriba && derecha && abajo)
                return TRight;
            
            else if (arriba && izquierda)
                return LeftUp;
                
            else if (arriba && derecha)
                return RightUp;
            
            else if (abajo && izquierda)
                return LeftDown;
            
            else if (abajo && derecha)
                return RightDown;
            
            else if (arriba)
                return Up;
            
            else if (abajo)
                return Up;
            
            else if (izquierda)
                return Left;
            
            else if (derecha)
                return Left;
            
            else
                return CenterNoConectionsOfTheWallForTheEnemiesToNotPassRightThroughBecauseThatWouldBeBadAndWeDontWantThatSoWeHaveToMakeSureThatTheEnemiesDontPassThroughTheWallsBecauseThatWouldBeBadAndWeDontWantThatSoWeHaveToMakeSureThatTheEnemiesDontPassThroughTheWalls;
            
        }
        
        if(this.cellIsPath)
            return EnemyPathSprite;
        
        return null;  
    }

    public void RemoveSprite()
    {
        spriteRenderer.sprite = null;
    }

    public void RemoveWall()
    {
        RemoveSprite();
        int[] coords = getCoords();
        textureCaller(coords[0],coords[1]);
    }

    public void MakeEnemyPath()
    {
        spriteRenderer.sprite = EnemyPathSprite;
    }
}

