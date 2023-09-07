using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //fix
    public float health;

    public Vector2Int position;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }

    public virtual void OnRoundStart()
    {

    }
}
