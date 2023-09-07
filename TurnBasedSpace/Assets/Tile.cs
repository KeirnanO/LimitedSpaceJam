using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public GameObject tileObject;
    public Vector2Int gridPos;

    private void OnMouseDown()
    {
        print("Clicked: " + gridPos.x + " , " + gridPos.y);
        Grid.instance.SpawnEntity(this, null);
    }

    private void OnMouseEnter()
    {
        Grid.instance.ShowTarget(gridPos.x, gridPos.y);
    }

    private void OnMouseExit()
    {
        Grid.instance.UnshowTarget();
    }
}
