using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scenes;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    private EnumCell type;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Start(){
        Resources.LoadAsync<Material>("Resources/Materials");
        // getting a reference to this cell material 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ChangeColor(Color col)
    {
        spriteRenderer.color = col;
    }
    
    // types EnemySpawn, Finish, Ground, Wall, Path, Tower, Obstacle
    public void ChangeTypes(EnumCell newType){
        type = newType;

        switch (type)
        {
            case EnumCell.Ground:
                spriteRenderer.material = Resources.Load<Material>("Materials/Ground");
                node.SetUsed(false);
                break;
            case EnumCell.Wall:
                spriteRenderer.material = Resources.Load<Material>("Materials/Wall");
                node.SetUsed(true);
                break;
            case EnumCell.Path:
                // TODO agregar material en unity
                //materialRef = Resources.Load<Material>("Materials/Path");
                node.SetUsed(false);
                break;
            case EnumCell.EnemySpawn:
                spriteRenderer.material = Resources.Load<Material>("Materials/EnemySpawn");
                node.SetUsed(false);
                break;
            case EnumCell.Finish:
                spriteRenderer.material = Resources.Load<Material>("Materials/Finish");
                node.SetUsed(false);
                break;
            case EnumCell.Tower:
                // TODO agregar material en UNITY
                // materialRef = Resources.Load<Material>("Materials/Tower");
                node.SetUsed(true);
                break;
            case EnumCell.Obstacle:
                // TODO agregar material en UNITY
                // materialRef = Resources.Load<Material>("Materials/Obstacle");
                node.SetUsed(true);
                break;            

            default:
                spriteRenderer.material = Resources.Load<Material>("Materials/Ground");
                break;
        }
    }
}
