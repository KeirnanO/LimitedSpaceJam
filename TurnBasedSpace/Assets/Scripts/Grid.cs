using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    public Tile gridPrefab;

    public GameObject rowImage;
    public GameObject columnImage;

    public GameObject unit1Preview;
    public GameObject unit2Preview;

    GameObject tempGo;
    public bool unitPreview = false;

    public string previewName = "NULL";

    public List<float> rowPositions;
    public List<float> columnPositions;

    public Tile[,] gridArray;
    public Vector3 gridScale;
    public Vector2 origin = new Vector2(0,0);

    public int rows;
    public int columns;

    private void Start()
    {
        instance = this;
        tempGo = new GameObject();
        UnshowTarget();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        gridArray = new Tile[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var obj = Instantiate(gridPrefab, transform);

                Vector2 position = new Vector2( columnPositions[i] * gridScale.x, rowPositions[j] * gridScale.y);

                gridArray[i, j] = obj;
                obj.transform.localScale = gridScale;
                obj.transform.position = position;


                obj.gridPos = new Vector2Int(i, j);
            }
        }
    }

    public Entity SpawnEntity(Tile tile, Entity entityPrefab)
    {
        if (tile.entity != null)
            return null;

        var newEntity = Instantiate(entityPrefab);
        tile.entity = newEntity;
        newEntity.transform.position = tile.transform.position;
        newEntity.transform.rotation = tempGo.transform.rotation;
        newEntity.position = tile.gridPos;

        return newEntity;
    }

    public Entity SpawnEntity(int x, int y, Entity entityPrefab)
    {
        //Uses more slightly more memory
        return SpawnEntity(gridArray[x, y], entityPrefab);
    }

    public void ShowTarget(int column, int row)
    {
        rowImage.SetActive(true);
        columnImage.SetActive(true);

        Vector3 rowPosition = rowImage.transform.position;
        rowPosition.y = rowPositions[row] * gridScale.x;

        Vector3 columnPosition = columnImage.transform.position;
        columnPosition.x = columnPositions[column] * gridScale.y;

        rowImage.transform.position = rowPosition;
        columnImage.transform.position = columnPosition;
    }
    public void ShowTarget(int column, int row, Entity prefabPreview)
    {
        unitPreview = true;

        rowImage.SetActive(true);
        columnImage.SetActive(true);

        Vector3 rowPosition = rowImage.transform.position;
        rowPosition.y = rowPositions[row] * gridScale.x;

        Vector3 columnPosition = columnImage.transform.position;
        columnPosition.x = columnPositions[column] * gridScale.y;

        rowImage.transform.position = rowPosition;
        columnImage.transform.position = columnPosition;

        Vector3 goPos = new Vector3(columnPosition.x, rowPosition.y, 0);

        switch (previewName)
        {
            case "PeaShooter":
                tempGo = unit1Preview;
                break;

            case "Flower":
                tempGo = unit2Preview;
                break;
        }


        tempGo.transform.position = goPos;
        tempGo.SetActive(true);
    }

    public void RotateTarget()
    {
        tempGo.transform.Rotate(0, 0, 90);
    }

    public void UnshowTarget()
    {
        rowImage.SetActive(false);
        columnImage.SetActive(false);
        tempGo.SetActive(false);
        unitPreview = false; 
    }
}

