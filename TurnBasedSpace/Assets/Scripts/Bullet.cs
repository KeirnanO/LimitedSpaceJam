using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * 5f;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            Entity enemy = collision.GetComponent<Entity>();
            enemy.TakeDamage(damage);

            GetComponent<Animator>().SetBool("Hit", true);
            GetComponent<BoxCollider2D>().enabled = false;

            GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0.5f, 1.5f), Random.Range(-0.2f, 0.2f));
        }
    }


}
