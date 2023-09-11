using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FIREDIRECTION
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}

public class Bullet : MonoBehaviour
{
    public int damage = 1;



    public FIREDIRECTION fireDirection = FIREDIRECTION.NORTH;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 5f;

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

            GetComponent<Rigidbody2D>().velocity *= .2f;
        }
    }


}
