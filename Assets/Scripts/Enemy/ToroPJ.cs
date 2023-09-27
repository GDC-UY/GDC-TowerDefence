using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToroPJ : MonoBehaviour
{
    private Enemy e = null;
    private Animator anim;
    private void Start()
    {
        e = this.GetComponent<Enemy>();
        anim = this.GetComponent<Animator>();
        this.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(10, 25);
    }

    void Update()
    {
        if (e.WalkingDirection == "RIGHT")
        {
            anim.SetInteger("Direction", 0);
        }
        else if (e.WalkingDirection == "DOWN")
        {
            anim.SetInteger("Direction", 1);
        }
        else if (e.WalkingDirection == "LEFT")
        {
            anim.SetInteger("Direction", 2);
        }
        else
        {
            anim.SetInteger("Direction", 3);
        }
        
    }
}
