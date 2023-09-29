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
        if (e.walkDirection.Equals(Enemy.WalkingDirection.Right))
        {
            anim.SetTrigger("Right");
        }
        else if (e.walkDirection.Equals(Enemy.WalkingDirection.Down))
        {
            anim.SetTrigger("Down");
        }
        else if (e.walkDirection.Equals(Enemy.WalkingDirection.Left))
        {
            anim.SetTrigger("Left");
        }
        else
        {
            anim.SetTrigger("Up");
        }
        
    }
}
