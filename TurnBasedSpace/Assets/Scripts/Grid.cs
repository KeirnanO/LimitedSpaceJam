using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    public Tile gridPrefab;
    public GameObject rowImage;
    public GameObject columnImage;

    public List<float> rowPositions;
    public List<float> columnPositions;

    public Tile[,] gridArray;
    public Vector3 gridScale;

    public int rows;
    public int columns;

    private void Start()
    {
        instance = this;
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

                Vector2 position = new Vector2( columnPositions[i], rowPositions[j]);

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
        rowPosition.y = rowPositions[row];

        Vector3 columnPosition = columnImage.transform.position;
        columnPosition.x = columnPositions[column];

        rowImage.transform.position = rowPosition;
        columnImage.transform.position = columnPosition;
    }

    public void UnshowTarget()
    {
        rowImage.SetActive(false);
        columnImage.SetActive(false);
    }
}
