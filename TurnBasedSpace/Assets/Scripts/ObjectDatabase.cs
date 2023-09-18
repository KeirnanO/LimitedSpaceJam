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

    public PlaceableScriptableObject GetObjectID(PlaceableScriptableObject so)
    {
        for(int i = 0; i < playerObjectDatabase.Count; i++)
        {
            if (playerObjectDatabase[i] == so)
                return playerObjectDatabase[i];
        }

        return null;
    }
}
