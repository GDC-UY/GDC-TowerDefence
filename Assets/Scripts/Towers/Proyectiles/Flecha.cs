using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    private Transform target;
    private float speed = 5;
    private int damage = 10;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.localPosition, target.position, speed * Time.deltaTime);
        //falta rotar la flecha para que apunte al enemigo
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
