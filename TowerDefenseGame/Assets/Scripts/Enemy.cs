using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GridManager manager;
    private LinkedList<Node> enemyPath;
    private LinkedListNode<Node> next;
    private GameObject nextGO;
    [SerializeField] private float enemySpeed = 2f; // Ajusta la velocidad según lo necesario
    [SerializeField] private bool isWalking = false; // Cambia "Walk" a "isWalking"
    [SerializeField] private int life = 100; // Ajusta la vida según lo necesario
    [SerializeField] private int bounty = 10; // Ajusta la recompensa por matar al enemigo
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        enemyPath = manager.GetPath();
        next = enemyPath.First;
        if (next != null)
            nextGO = next.Value.GetValue();
    }

    private float deltaX;
    private float deltaY; 
    
    private void Update()
    {
        if (isWalking && next != null)
        {
            Vector3 targetPosition = nextGO.transform.position;
            Vector3 currentPosition = transform.position;

            deltaX = targetPosition.x - currentPosition.x;
            deltaY = targetPosition.y - currentPosition.y;

            if (Mathf.Abs(deltaX) < enemySpeed * Time.deltaTime && Mathf.Abs(deltaY) < enemySpeed * Time.deltaTime)
            {
                transform.position = new Vector2(targetPosition.x, targetPosition.y);
                next.Value.GetValue().GetComponent<SpriteRenderer>().color = Color.blue;
                next = next.Next;
                if (next != null)
                {
                    nextGO = next.Value.GetValue();
                    next.Value.GetValue().GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    next = enemyPath.First;
                }
            }
            else
            {
                Vector3 moveDirection = new Vector3(Mathf.Sign(deltaX), Mathf.Sign(deltaY), 0);
                if (Mathf.Abs(deltaY) >= enemySpeed * Time.deltaTime)
                {
                    moveDirection.x = 0;
                }
                else
                {
                    moveDirection.y = 0;
                }

                MoveAdd(moveDirection.x * enemySpeed * Time.deltaTime, moveDirection.y * enemySpeed * Time.deltaTime);
            }
        }
    }

    private void MoveAdd(float x, float y)
    {
        Vector3 pos = transform.position;
        transform.position = pos + new Vector3(x, y, 0);
    }

    // Agrega un método para iniciar el movimiento del enemigo
    public void StartWalking()
    {
        isWalking = true;
    }

    public void StopWalking()
    {
        isWalking = false;
    }
    
    public void Dead()
    {
        Destroy(gameObject);
        GameManager.instance.AddMoney(bounty);
    }
    
    //how to create a method to reduce its life when it is hit by a bullet
    public void Hit(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Dead();
        }
    }  

}