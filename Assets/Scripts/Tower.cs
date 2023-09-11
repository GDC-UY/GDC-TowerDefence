using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform target;
    GameObject playerBase;
    [SerializeField] LinkedList<GameObject> enemiesInRange = new LinkedList<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        playerBase = GameObject.Find("19x19");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// De todos los enemigos, encuentra el más cercano a la torre y lo targetea. Si no hay ninguno, el target es null
    /// </summary>
    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distanceEnemyToBase = Vector3.Distance(playerBase.transform.position, enemy.transform.position);
            
            if (distanceEnemyToBase < shortestDistance)
            {
                shortestDistance = distanceEnemyToBase;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemiesInRange.AddLast(collision.gameObject);
            UpdateTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemiesInRange.Remove(collision.gameObject);
            UpdateTarget();
        }
    }
}
