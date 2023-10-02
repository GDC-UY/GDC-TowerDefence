using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanza : MonoBehaviour
{
    GameObject abajo, costado;

    [SerializeField] private int cost;

    private int damage = 20;

    private void Start()
    {
        abajo = transform.GetChild(0).gameObject;
        costado = transform.GetChild(1).gameObject;
    }

    public int getCost()
    {
        return cost;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(Attack());
            collision.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
        }
    }

    IEnumerator Attack()
    {
        abajo.transform.localScale = new Vector3(0.2f, 3.6f, 0);
        costado.transform.localScale = new Vector3(3.6f, 0.2f, 0);

        yield return new WaitForSeconds(0.4f);

        abajo.transform.localScale = new Vector3(0.2f, 0.2f, 0);
        costado.transform.localScale = new Vector3(0.2f, 0.2f, 0);

        yield return null;
    }
}
