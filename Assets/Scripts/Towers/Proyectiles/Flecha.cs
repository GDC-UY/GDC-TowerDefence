using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    private Transform target;
    private float speed = 5;
    private int damage = 20;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.localPosition, target.position, speed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.ReceiveDamage(damage);
            if (enemy.health <= 0)
            {
                gameObject.transform.parent.gameObject.GetComponent<Arqueros>().UpdateTarget();
            }
            Destroy(gameObject);
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
