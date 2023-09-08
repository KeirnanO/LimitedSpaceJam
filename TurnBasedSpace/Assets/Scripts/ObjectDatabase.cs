using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDatabase : MonoBehaviour
{
    public static ObjectDatabase instance;

    public List<PlaceableScriptableObject> playerObjectDatabase;

    private void Awake()
    {
        instance = this;
    }
}
