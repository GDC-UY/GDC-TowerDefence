using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform target;
    float range = 2;
    GameObject playerBase;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceTower = Vector3.Distance(transform.position, enemy.transform.position);
            float distanceBase = Vector3.Distance(playerBase.transform.position, enemy.transform.position);

            if (distanceBase < shortestDistance && distanceTower <= range)
            {
                shortestDistance = distanceBase;
                nearestEnemy = enemy;
                Debug.Log("Enemigo: " + nearestEnemy.name + "\ndistancia torre " + distanceTower + "\ndistancia baase " + distanceBase);

            }

            if (nearestEnemy != null)
            {
                target = enemy.transform;
            }
            else
            {
                target = null;
            }
        }
        /*foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, range);
    }
}
