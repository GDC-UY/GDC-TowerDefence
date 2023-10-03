using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcoAceite : MonoBehaviour
{
    LinkedList<GameObject> enemiesInRange = new LinkedList<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TiempoDeVida());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemiesInRange.AddLast(collision.gameObject);
            collision.GetComponent<Enemy>().speed /= 2;
        }
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
            item.GetComponent<Enemy>().speed *= 2;
        }
        Destroy(gameObject);
    }
}
