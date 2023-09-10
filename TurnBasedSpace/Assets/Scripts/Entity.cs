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

public enum DirSpawned
{
    NORTH = 0,
    SOUTH = 1,
    EAST = 2,
    WEST = 3,
    NONE = 4

}


public class Entity : MonoBehaviour
{
    //Temp way to give a like attribute between the same entity;
    public PlaceableScriptableObject objectID;

    //fix
    public float health;
    public string UnitName = "NULL";

    public Vector2Int position;
    public EntityType type;
    public DirSpawned dirSpawned = DirSpawned.NONE;

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

    public void SetDir(DirSpawned ds) { dirSpawned = ds; }

    public virtual void OnRoundStart()
    {

    }

    public void PathToOrigin()
    {

    }

    public void DestroyEntity()
    {
        GameManager.instance.DestroyEntity(this);
    }
}
