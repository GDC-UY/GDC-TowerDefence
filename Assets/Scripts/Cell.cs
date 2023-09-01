using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scenes;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Node node;
    private Boolean canWalk;
    private EnumCell type;
    public void ChangeTypes(EnumCell newType){
        type = newType;

        switch (type)
        {
            case EnumCell.Wall:
                canWalk = false;
                // TODO: change type
                break;

            default:
                break;
        }
    }
}

