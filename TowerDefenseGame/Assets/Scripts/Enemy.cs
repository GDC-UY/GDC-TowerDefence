using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GridManager manager;
    private LinkedList<Node> enemyPath;
    
    private LinkedListNode<Node> next;
    [SerializeField] private float enemySpeed = 0.01f;
    [SerializeField] private bool Walk = false;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        enemyPath = manager.GetPath();

        next = enemyPath.First;
    }

    // Update is called once per frame
    float threshold = 0.01f;
    void Update()
    {
        if (Walk && next != null)
        {
            Vector3 targetPosition = next.Value.GetValue().transform.position;
            Vector3 currentPosition = transform.position;

            float deltaX = targetPosition.x - currentPosition.x;
            float deltaY = targetPosition.y - currentPosition.y;

            if (Mathf.Abs(deltaX) < threshold && Mathf.Abs(deltaY) < threshold)
            {
                transform.position = new Vector3(targetPosition.x, targetPosition.y, currentPosition.z);
                next.Value.GetValue().GetComponent<SpriteRenderer>().color = Color.blue;
                next = next.Next;
                if(next != null)
                    next.Value.GetValue().GetComponent<SpriteRenderer>().color = Color.yellow;
                else
                {
                    next = enemyPath.First;
                }
            }
            else
            {
                Vector3 moveDirection = new Vector3(Mathf.Sign(deltaX), Mathf.Sign(deltaY), 0);

                MoveAdd(moveDirection.x * enemySpeed, moveDirection.y * enemySpeed);
            }
        }
    }

    void MoveAdd(float x, float y)
    {
        Vector3 pos = transform.position;
        transform.position = pos +  new Vector3(x, y, 0);
    }
}
