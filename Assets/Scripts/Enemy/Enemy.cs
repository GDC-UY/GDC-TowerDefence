using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    //------ Enemy properties -------
    public int health = 100; //Vida del enemigo
    public float speed = 2f; //Velocidad base del enemigo
    public int damage = 1; //Daño que hace el enemigo a la base del jugador
    public int cost = 11; //Costo de invocación del enemigo (USO INTERNO)

    public int gold = 10; //Oro que deja al morir

    public bool isWalking = false; // Cambia "Walk" a "isWalking"

    private GridManager manager;
    private LinkedList<Node> enemyPath;
    private LinkedListNode<Node> next;
    private GameObject nextGO;
    public WalkingDirection walkDirection;

    public Sprite walk1, walk2, walk3, up1, up2, up3, down1, down2, down3;

    private SpriteRenderer spriteRenderer;

    bool walkRight = true;

    public enum WalkingDirection
    {
        Up,
        Down, 
        Left,
        Right
    }
    
    public void death()
    {
        this.gameObject.transform.parent.GetComponent<EnemySummoner>().enemyDied();
        Destroy(this.gameObject);
        //return gold -/- game.RecieveMoney(gold) //Cuando muere debe retornar o llamar al metodo para que el jugador reciba oro. Por ahora no estan vinculados los archivos.
    }
    
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        enemyPath = manager.GetPath();
        next = enemyPath.First;
        if (next != null)
            nextGO = next.Value.GetValue();
        
        // Genera un offset aleatorio
        offSetX = Random.Range(-0.25f, 0.25f);
        offSetY = Random.Range(-0.25f, 0.25f);

        walkDirection = WalkingDirection.Right;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (!gameObject.name.Contains("Toro"))
        {
            StartCoroutine(WalkAnimation());
        }
    }
    
    private float deltaX;
    private float deltaY;
    
    private float offSetX;
    private float offSetY;
    
    private void Update()
    {
        if(health <= 0 )
            death();
        
        if (isWalking && next != null)
        {
            // Obtiene la posición objetivo con un desplazamiento aleatorio
            Vector3 targetPosition = nextGO.transform.position + new Vector3(offSetX, offSetY, 0);
            
            Vector3 currentPosition = transform.position;
    
            deltaX = targetPosition.x - currentPosition.x;
            deltaY = targetPosition.y - currentPosition.y;
    
            if (Mathf.Abs(deltaX) < speed * Time.deltaTime && Mathf.Abs(deltaY) < speed * Time.deltaTime)
            {
                transform.position = new Vector2(targetPosition.x, targetPosition.y);
                next = next.Next;
                if (next != null)
                {
                    nextGO = next.Value.GetValue();
                }
                else
                {
                    death();
                }
            }
            else
            {
                Vector3 moveDirection = new Vector3(Mathf.Sign(deltaX), Mathf.Sign(deltaY), 0);
                if (Mathf.Abs(deltaY) >= speed * Time.deltaTime)
                {
                    moveDirection.x = 0;
                }
                else
                {
                    moveDirection.y = 0;
                }
                MoveAdd((moveDirection.x * speed * Time.deltaTime), (moveDirection.y * speed * Time.deltaTime));
            }
        }
    }
    
    private void MoveAdd(float x, float y)
    {
        Vector3 pos = transform.position;
        transform.position = pos + new Vector3(x, y, 0);
        
        if (x > 0)
        {
            walkDirection = WalkingDirection.Right;
        }
        else if (x < 0)
        {
            walkDirection = WalkingDirection.Left;
        }
        else if (y > 0)
        {
            walkDirection = WalkingDirection.Up;
        }
        else if (y < 0)
        {
            walkDirection = WalkingDirection.Down;
        }
    }

    IEnumerator WalkAnimation()
    {
        while (true)
        {
            switch (walkDirection)
            {
                case WalkingDirection.Right:
                    if (!walkRight)
                    {
                        walkRight = true;
                        gameObject.transform.Rotate(0, 180, 0);
                    }
                    spriteRenderer.sprite = walk1;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = walk2;
                    yield return new WaitForSeconds(0.1f);
                    spriteRenderer.sprite = walk3;
                    yield return new WaitForSeconds(0.2f);
                    break;
                case WalkingDirection.Left:
                    if (walkRight)
                    {
                        walkRight = false;
                        gameObject.transform.Rotate(0, 180, 0);
                    }
                    spriteRenderer.sprite = walk1;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = walk2;
                    yield return new WaitForSeconds(0.1f);
                    spriteRenderer.sprite = walk3;
                    yield return new WaitForSeconds(0.2f);
                    break;
                case WalkingDirection.Up:
                    spriteRenderer.sprite = up1;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = up2;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = up3;
                    yield return new WaitForSeconds(0.2f);
                    break;
                case WalkingDirection.Down:
                    spriteRenderer.sprite = down1;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = down2;
                    yield return new WaitForSeconds(0.2f);
                    spriteRenderer.sprite = down3;
                    yield return new WaitForSeconds(0.2f);
                    break;
            }
        }
    }
}
