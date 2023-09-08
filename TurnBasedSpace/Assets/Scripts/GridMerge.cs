using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridMerge
{

    public static bool TrySwapTile(int x, int y)
    {
        if (x > Grid.instance.columns - 1)
            return false;

        Tile[,] gridArray = Grid.instance.gridArray;

        int x2 = x + 1;

        //Check if tile1 swap makes combo
        List<Tile> newChain = null;
        List<Tile> newChain2 = null;


        if (gridArray[x, y].entity != null)
        {
            PlaceableScriptableObject objectID = gridArray[x, y].entity.objectID;

            newChain = GetChain(x, y, gridArray, objectID);
        }


        //Check if tile2 swap makes combo
        if (gridArray[x2, y].entity != null)
        {
            PlaceableScriptableObject objectID2 = gridArray[x2, y].entity.objectID;

            newChain2 = GetChain(x2, y, gridArray, objectID2);        
        }

        //Return out if no chains
        if (newChain == null && newChain2 == null)
            return false;

        //If there are chains swap the tiles
        SwapTiles(x, y, gridArray);
        return true;
    }

    //Ugly but works
    static void SwapTiles(int x, int y, Tile[,] grid)
    {
        Entity tempEntity = grid[x, y].entity;
        int x2 = x + 1;


        grid[x, y].entity = grid[x2, y].entity;

        if (grid[x2, y].entity != null)
        {
            grid[x2, y].entity.transform.position = grid[x, y].transform.position;
        }

        
        grid[x2, y].entity = tempEntity;

        if (tempEntity != null)
        {
            grid[x2, y].entity.transform.position = grid[x2, y].transform.position;
        }
    }

    static List<Tile> GetChain(int x, int y, Tile[,] grid, PlaceableScriptableObject ID)
    {
        List<Tile> chain = new List<Tile>();

        PlaceableScriptableObject comparedObjectID;

        //The first slot in the chain will be the swapped tile
        chain.Add(grid[x, y]);

        //Search Down
        if (y > 0)
        {
            //Add keep adding tiles to the chain until there isnt a match
            for (int y2 = y - 1; y >= 0; y--)
            {
                if (grid[x, y2].entity == null)
                    break;

                comparedObjectID = grid[x, y2].entity.objectID;

                if (comparedObjectID.Equals(ID))
                {
                    chain.Add(grid[x, y2]);
                    continue;
                }
                else 
                    break;
            }
        }

        //Search Up
        if (y < Grid.instance.rows - 1)
        {
            //Add keep adding tiles to the chain until there isnt a match
            for (int y2 = y + 1; y < Grid.instance.rows; y++)
            {
                if (grid[x, y2].entity == null)
                    break;

                comparedObjectID = grid[x, y2].entity.objectID;

                if (comparedObjectID.Equals(ID))
                {
                    chain.Add(grid[x, y2]);
                    continue;
                }
                else
                    break;
            }
        }

        //Delete Chain if there is less than 3 matches **not including the originalPeice**
        if (chain.Count < 3)
            chain.Clear();

        return chain;
    }

}
