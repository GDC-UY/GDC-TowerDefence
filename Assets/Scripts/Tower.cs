using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform target;
    GameObject playerBase;
    LinkedList<GameObject> enemiesInRange = new LinkedList<GameObject>();

    [SerializeField] private int cost;

    public GameObject projectile;

    private float attackSpeed = 1;

    public int getCost() {
        return cost;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("Shoot", 0f, attackSpeed);
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

    private void Shoot()
    {
        if (target != null)
        {
            GameObject projectileToShoot = Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            projectileToShoot.GetComponent<Flecha>().SetTarget(target);
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
