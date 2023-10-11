using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcoAceite : MonoBehaviour
{
    LinkedList<GameObject> enemiesInRange = new LinkedList<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy  enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.gameObject.tag == "Enemy" && !enemy.isSlowed)
        {
            enemiesInRange.AddLast(collision.gameObject);
            enemy.speed /= 2;
            enemy.isSlowed = true;
        }
        StartCoroutine(TiempoDeVida());
    }

    IEnumerator TiempoDeVida()
    {
        yield return new WaitForSeconds(3);
        DesaparecerCharco();
    }

    void DesaparecerCharco()
    {
        foreach (var item in enemiesInRange)
        {
            Enemy enemy = item.GetComponent<Enemy>();
            if (enemy.isSlowed)
            {
                enemy.speed*=2;
                enemy.isSlowed = false;
            }
            Debug.Log(enemy.name);
        }
        Destroy(gameObject);
    }
}
