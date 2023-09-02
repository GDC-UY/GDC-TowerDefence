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
    private Material materialRef; 
    private void Start(){
        ResourceRequest materials = Resources.LoadAsync<Material>("Resources/Materials");
        // getting a reference to this cell material 
        materialRef = GetComponent<Material>();
    }
    // types EnemySpawn, Finish, Ground, Wall, Path, Tower, Obstacle
    public void ChangeTypes(EnumCell newType){
        type = newType;

        switch (type)
        {
            case EnumCell.Ground:
                materialRef = Resources.Load<Material>("Materials/Ground");
                node.SetUsed(false);
                break;
            case EnumCell.Wall:
                materialRef = Resources.Load<Material>("Materials/Wall");
                node.SetUsed(true);
                break;
            case EnumCell.Path:
                // TODO agregar material en unity
                //materialRef = Resources.Load<Material>("Materials/Path");
                node.SetUsed(false);
                break;
            case EnumCell.EnemySpawn:
                materialRef = Resources.Load<Material>("Materials/EnemySpawn");
                node.SetUsed(false);
                break;
            case EnumCell.Finish:
                materialRef = Resources.Load<Material>("Materials/Finish");
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
                materialRef = Resources.Load<Material>("Materials/Ground");
                break;
        }
    }
}
