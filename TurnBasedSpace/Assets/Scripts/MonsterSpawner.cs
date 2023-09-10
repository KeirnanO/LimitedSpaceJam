using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance { get; private set; }

    public List<Vector2> NorthSpawnPoints = new List<Vector2>();
    public List<Vector2> SouthSpawnPoints = new List<Vector2>();
    public List<Vector2> EastSpawnPoints = new List<Vector2>();
    public List<Vector2> WestSpawnPoints = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null);
    }

    
}
