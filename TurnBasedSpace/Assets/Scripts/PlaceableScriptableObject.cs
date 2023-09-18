using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretScriptableObject", menuName = "ScriptableObjects/Turret", order = 1)]
public class PlaceableScriptableObject : ScriptableObject
{
    public int cost;
    public string Name;
    public Sprite icon;

    public Entity EntityPrefab;
    public PlaceableScriptableObject upgradedUnit;
}
