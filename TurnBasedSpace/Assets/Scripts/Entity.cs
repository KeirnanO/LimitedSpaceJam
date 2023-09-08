using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType 
{
    Normal = 0,
    Plant = 1,
    Enemy = 2,
    Obstacle = 3
}


public class Entity : MonoBehaviour
{
    //fix
    public float health;

    public Vector2Int position;
    public EntityType type;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            if(GetComponent<BoxCollider2D>())
                GetComponent<BoxCollider2D>().enabled = false;


            if (GetComponent<Animator>())
                GetComponent<Animator>().SetTrigger("Die");
        }
    }

    public virtual void OnRoundStart()
    {

    }

    public void DestroyEntity()
    {
        GameManager.instance.DestroyEntity(this);
    }
}
