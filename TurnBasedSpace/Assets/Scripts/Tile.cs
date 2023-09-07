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
        GameManager.instance.ClickTile(this);
    }

    private void OnMouseEnter()
    {
        GameManager.instance.ShowTarget(gridPos.x, gridPos.y);
    }

    private void OnMouseExit()
    {
        GameManager.instance.UnShowTarget();
    }
}
